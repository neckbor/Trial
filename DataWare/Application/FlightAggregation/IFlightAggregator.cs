using Application.FlightAggregation.DTOs;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Shared;

namespace Application.FlightAggregation;

public interface IFlightAggregator
{
    Task<Result> AggregateAsync(SearchRequestDto request);
    Task<Result<BaseFlight>> GetFlightByIdAsync(SearchRequest request, string flightId);
    Task<Result<SearchResult>> GetSearchResultAsync(string searchKey);
}