using Application.FlightAggregation.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Shared;

namespace Application.FlightAggregation;

public interface IFlightAggregator
{
    Task<Result> AggregateAsync(SearchRequest request);
    Task<Result<BaseFlight>> GetFlightByIdAsync(SearchRequest request, string flightId);
    Task<Result<SearchResult>> GetSearchResultAsync(string searchKey);
}