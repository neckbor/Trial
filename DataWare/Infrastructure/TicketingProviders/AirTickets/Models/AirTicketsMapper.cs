using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Application.FlightSearch;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Infrastructure.TicketingProviders.AirTickets.Models;

internal class AirTicketsMapper
{
    private readonly TicketingProvider _provider;
    private readonly IAirlineService _airlineService;
    private readonly IAirportService _airportService;

    public AirTicketsMapper(TicketingProvider provider, IAirlineService airlineService, IAirportService airportService)
    {
        _provider = provider;
        _airlineService = airlineService;
        _airportService = airportService;
    }

    public async Task<Result<BaseFlight>> Map(AirTicketsFlight source)
    {
        var segments = new List<BaseSegment>();

        foreach (var s in source.Segments)
        {
            var getAirlineResult = await _airlineService.GetByICAOCodeAsync(s.AirlineCode);

            if (getAirlineResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getAirlineResult.Error);
            }

            var airline = getAirlineResult.Value;

            var getFromAirportResult = await _airportService.GetByIATACodeAsync(s.From);
            if (getFromAirportResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getFromAirportResult.Error);
            }

            var fromAirport = getFromAirportResult.Value;

            var getToAirportResult = await _airportService.GetByIATACodeAsync(s.To);
            if (getToAirportResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getToAirportResult.Error);
            }

            var toAirport = getToAirportResult.Value;

            segments.Add(new BaseSegment
            {
                FlightNumber = s.Number,
                Airline = airline,
                From = fromAirport,
                To = toAirport,
                DepartureDateUtc = s.DepartureUtc,
                ArrivalDateUtc = s.ArrivalUtc,
                AvailableSeats = s.AvailableSeats
            });
        }

        var baseFlight = new BaseFlight
        {
            FlightId = source.Id,
            TicketingProvider = _provider,
            Fare = new FareDetails
            {
                BaseFare = source.Price,
                Taxes = 0
            },

            Segments = segments
        };

        return Result.Success(baseFlight);
    }
}
