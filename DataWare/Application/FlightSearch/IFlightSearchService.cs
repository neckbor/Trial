using Application.FlightAggregation.DTOs;
using Application.FlightSearch.DTOs;
using Domain.Shared;

namespace Application.FlightSearch;

public interface IFlightSearchService
{
    Task<Result<Guid>> CreateSearchRequestAsync(StartSearchCommand command);
    Task<Result> LaunchFlightSearchAsync();
    Task<Result<SearchResult>> GetSearchResultAsync(SearchResultsQuery query);
}
