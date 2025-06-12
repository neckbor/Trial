using Application.Dictionaries.Airports;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Shared;

namespace Application.FlightSearch;

internal class FlightSearchService : IFlightSearchService
{
    private readonly IAirportService _airportService;

    public FlightSearchService(IAirportService airportService)
    {
        _airportService = airportService;
    }

    public async Task<Result<Guid>> StartSearch(StartSearchCommand command)
    {
        var getFromAirportResult = await _airportService.GetByIATACodeAsync(command.FromAirportIATACode);
        if (getFromAirportResult.IsFailure) 
        {
            // log error
            return Result.Failure<Guid>(getFromAirportResult.Error);
        }
        var fromAirport = getFromAirportResult.Value;

        var getToAirportResult = await _airportService.GetByIATACodeAsync(command.ToAirportIATACode);
        if (getToAirportResult.IsFailure) 
        {
            // log error
            return Result.Failure<Guid>(getToAirportResult.Error);
        }
        var toAirport = getToAirportResult.Value;

        var createSearchRequestResult = SearchRequest.Create(command.ClientId, fromAirport, toAirport, command.DepartureDate, command.PassengerCount);
        if (createSearchRequestResult.IsFailure) 
        {
            // log error
            return Result.Failure<Guid>(createSearchRequestResult.Error);
        }
        var searchRequest = createSearchRequestResult.Value;


        return searchRequest.Id;
    }
}
