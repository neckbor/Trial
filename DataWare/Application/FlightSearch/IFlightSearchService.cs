using Application.FlightSearch.DTOs;
using Domain.Shared;

namespace Application.FlightSearch;

public interface IFlightSearchService
{
    Task<Result<string>> CreateSearchRequestAsync(StartSearchCommand command);
}
