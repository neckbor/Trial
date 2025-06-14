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

    public FlightAggregator(ISearchResultCache searchResultCache)
    {
        _searchResultCache = searchResultCache;
    }

    public Task<Result> AggregateAsync(SearchRequest request)
    {
        throw new NotImplementedException();
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
}
