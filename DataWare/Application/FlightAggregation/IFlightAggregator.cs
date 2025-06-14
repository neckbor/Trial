using Application.FlightAggregation.DTOs;
using Domain.Entities;
using Domain.Shared;

namespace Application.FlightAggregation;

public interface IFlightAggregator
{
    Task<Result> AggregateAsync(SearchRequest request);
    Task<Result<SearchResult>> GetSearchResultAsync(string searchKey);
}