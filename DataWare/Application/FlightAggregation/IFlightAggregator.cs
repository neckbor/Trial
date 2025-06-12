using Domain.Entities;
using Domain.Models;
using Domain.Shared;

namespace Application.FlightAggregation;

public interface IFlightAggregator
{
    Task<Result<List<BaseFlight>>> AggregateAsync(SearchRequest request, CancellationToken cancellationToken);
}