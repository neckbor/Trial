using Application.FlightAggregation.DTOs;
using Application.InfrastructureAbstractions;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.FlightAggregation;

internal class FlightAggregator : IFlightAggregator
{
    private readonly ILogger<FlightAggregator> _logger;
    private readonly ISearchResultCache _searchResultCache;
    private readonly IEnumerable<ITicketingProvider> _ticketingProviders;

    public FlightAggregator(
        ILogger<FlightAggregator> logger, 
        ISearchResultCache searchResultCache, 
        IEnumerable<ITicketingProvider> ticketingProviders)
    {
        _searchResultCache = searchResultCache;
        _ticketingProviders = ticketingProviders;
        _logger = logger;
    }

    public async Task<Result<SearchResult>> GetSearchResultAsync(string searchKey)
    {
        var getCachedFlightsResult = await _searchResultCache.GetFlightsAsync(searchKey);
        if (getCachedFlightsResult.IsFailure)
        {
            _logger.LogWarning("Ошибка при получении результатов поиска по ключу {SearchKey} {ErrorCode}: {ErrorMessage}",
                searchKey,
                getCachedFlightsResult.Error.Code,
                getCachedFlightsResult.Error.Message);

            return SearchResult.Fail(getCachedFlightsResult.Error);
        }

        List<BaseFlight> flights = getCachedFlightsResult.Value;

        var getCachedProviderSearchStatusesResult = await _searchResultCache.GetProviderSearchStatusAsync(searchKey);
        if (getCachedProviderSearchStatusesResult.IsFailure)
        {
            _logger.LogWarning("Ошибка при получении статусов провайдера поиска по ключу {SearchKey} {ErrorCode}: {ErrorMessage}",
                searchKey,
                getCachedProviderSearchStatusesResult.Error.Code,
                getCachedProviderSearchStatusesResult.Error.Message);

            return SearchResult.Fail(getCachedProviderSearchStatusesResult.Error);
        }

        Dictionary<TicketingProvider, SearchStatus> providerSearchStatuses = getCachedProviderSearchStatusesResult.Value;

        if (providerSearchStatuses.Count == 0 || providerSearchStatuses.Values.Any(s => s == SearchStatus.Pending))
        {
            return SearchResult.Pending(flights);
        }

        if (flights.Count == 0)
        {
            return SearchResult.Fail(FlightAggregationErrors.FlightsNotFound);
        }

        return SearchResult.Completed(flights);
    }

    public async Task<Result> AggregateAsync(SearchRequest request)
    {
        var searchKey = request.SearchResultKey;

        var getFlightsTtlResult = await _searchResultCache.GetFlightsTtlAsync(searchKey);
        if (getFlightsTtlResult.IsSuccess)
        {
            var flightsTtl = getFlightsTtlResult.Value;
            if (flightsTtl > TimeSpan.FromMinutes(1))
            {
                _logger.LogInformation("Время жизни результатов поиска {SearchKey} позволяет не запускать поиск у провайдеров", searchKey);
                return Result.Success();
            }
        }

        var clearResult = await _searchResultCache.CLearAsync(searchKey);
        if (clearResult.IsFailure)
        {
            _logger.LogWarning(
                "При очистке кеша по ключу {SearchKey} произошла ошибка {ErrorCode}: {ErrorMessage}",
                searchKey,
                clearResult.Error.Code,
                clearResult.Error.Message);

            return Result.Failure(clearResult.Error);
        }

        // seed search key with empty flights
        await _searchResultCache.AddFlightsAsync(searchKey, new List<BaseFlight>());

        foreach (var provider in _ticketingProviders) 
        {
            await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Pending);

            _ = Task.Run(() => RunProviderAsync(provider, request));
        }

        return Result.Success();
    }

    private async Task RunProviderAsync(ITicketingProvider provider, SearchRequest request)
    {
        var searchKey = request.SearchResultKey;

        try
        {
            var searchResult = await provider.SearchAsync(request);
            if (searchResult.IsFailure)
            {
                _logger.LogWarning("Ошибка при поиске перелётов в провайдере {TicketingProvider} {ErrorCode}: {ErrorMessage}",
                    provider.Provider.Code,
                    searchResult.Error.Code,
                    searchResult.Error.Message);

                await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Failed);
                return;
            }

            var flights = searchResult.Value;

            await _searchResultCache.AddFlightsAsync(searchKey, flights);
            await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Completed);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex,
                "Исключеине при поиске перелётов в провайдере {TicketingProvider}",
                provider.Provider.Code);

            await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Failed);
        }
    }
}
