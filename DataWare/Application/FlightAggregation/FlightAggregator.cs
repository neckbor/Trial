using Application.FlightAggregation.DTOs;
using Application.InfrastructureAbstractions;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Application.FlightAggregation;

internal class FlightAggregator : IFlightAggregator
{
    private readonly ISearchResultCache _searchResultCache;
    private readonly IEnumerable<ITicketingProvider> _ticketingProviders;

    public FlightAggregator(ISearchResultCache searchResultCache, IEnumerable<ITicketingProvider> ticketingProviders)
    {
        _searchResultCache = searchResultCache;
        _ticketingProviders = ticketingProviders;
    }

    public async Task<Result<SearchResult>> GetSearchResultAsync(string searchKey)
    {
        var getCachedFlightsResult = await _searchResultCache.GetFlightsAsync(searchKey);
        if (getCachedFlightsResult.IsFailure)
        {
            return SearchResult.Fail(getCachedFlightsResult.Error);
        }

        List<BaseFlight> flights = getCachedFlightsResult.Value;

        var getCachedProviderSearchStatusesResult = await _searchResultCache.GetProviderSearchStatusAsync(searchKey);
        if (getCachedProviderSearchStatusesResult.IsFailure)
        {
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
                await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Failed);
                return;
            }

            var flights = searchResult.Value;

            await _searchResultCache.AddFlightsAsync(searchKey, flights);
            await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Completed);
        }
        catch (Exception ex) 
        {
            await _searchResultCache.SetProviderSearchStatusAsync(searchKey, provider.Provider, SearchStatus.Failed);
        }
    }
}
