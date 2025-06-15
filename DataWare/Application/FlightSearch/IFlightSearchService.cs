using Application.FlightAggregation.DTOs;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Shared;

namespace Application.FlightSearch;

public interface IFlightSearchService
{
    Task<Result<Guid>> CreateSearchRequestAsync(StartSearchCommand command);
    Task<Result> LaunchFlightSearchAsync();
    Task<Result<SearchResult>> GetSearchResultAsync(GetSearchResultsQuery query);
    Task<Result<SearchRequest>> GetSearchRequestByIdAsync(Guid searchRequestId);
}
