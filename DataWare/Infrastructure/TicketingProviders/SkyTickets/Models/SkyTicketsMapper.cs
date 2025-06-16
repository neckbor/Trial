using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Infrastructure.TicketingProviders.SkyTickets.Models;

internal class SkyTicketsMapper
{
    private readonly TicketingProvider _provider;
    private readonly IAirlineService _airlineService;
    private readonly IAirportService _airportService;

    public SkyTicketsMapper(TicketingProvider provider, IAirlineService airlineService, IAirportService airportService)
    {
        _provider = provider;
        _airlineService = airlineService;
        _airportService = airportService;
    }

    public async Task<Result<BaseFlight>> Map(SkyTicketsFlight source)
    {
        var segments = new List<BaseSegment>();

        foreach(var leg in source.Legs)
        {
            var getAirlineResult = await _airlineService.GetByIATACodeAsync(leg.Airline);

            if (getAirlineResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getAirlineResult.Error);
            }

            var airline = getAirlineResult.Value;

            var getFromAirportResult = await _airportService.GetByIATACodeAsync(leg.Origin);
            if (getFromAirportResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getFromAirportResult.Error);
            }

            var fromAirport = getFromAirportResult.Value;

            var getToAirportResult = await _airportService.GetByIATACodeAsync(leg.Destination);
            if (getToAirportResult.IsFailure)
            {
                return Result.Failure<BaseFlight>(getToAirportResult.Error);
            }

            var toAirport = getToAirportResult.Value;

            if (!DateTime.TryParse(leg.DepUtc, out var departure) || !DateTime.TryParse(leg.ArrUtc, out var arrival))
            {
                return Result.Failure<BaseFlight>(TicketingProviderErrors.ParsingFailed(_provider));
            }

            segments.Add(new BaseSegment
            {
                FlightNumber = leg.FlightNumber,
                Airline = airline,
                From = fromAirport,
                To = toAirport,
                DepartureDateUtc = departure,
                ArrivalDateUtc = arrival,
                AvailableSeats = leg.AvailableSeats
            });
        }

        var baseFlight = new BaseFlight()
        {
            FlightId = source.Id.ToString(),
            TicketingProvider = _provider,
            Fare = new FareDetails { BaseFare = (decimal)source.Price, Taxes = 0 },
            Segments = segments
        };

        return baseFlight;
    }
}
