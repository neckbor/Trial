using Application.FlightSearch.DTOs;
using Domain.Shared;

namespace Application.FlightSearch;

public interface IFlightSearchService
{
    Task<Result<Guid>> StartSearch(StartSearchCommand command);
}
