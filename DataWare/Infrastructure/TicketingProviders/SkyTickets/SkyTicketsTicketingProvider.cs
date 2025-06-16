using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Application.FlightSearch.DTOs;
using Application.InfrastructureAbstractions;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;
using Infrastructure.TicketingProviders.SkyTickets.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.TicketingProviders.SkyTickets;

internal class SkyTicketsTicketingProvider : ITicketingProvider
{
    public TicketingProvider Provider => TicketingProvider.SkyTickets;

    private readonly ILogger<SkyTicketsTicketingProvider> _logger;
    private readonly IAirlineService _airlineService;
    private readonly IAirportService _airportService;

    public SkyTicketsTicketingProvider(ILogger<SkyTicketsTicketingProvider> logger, IAirlineService airlineService, IAirportService airportService)
    {
        _logger = logger;
        _airlineService = airlineService;
        _airportService = airportService;
    }

    public Task<Result<BaseBooking>> BookAsync(string flightId, List<Passenger> passengers)
    {
        return Task.FromResult(Result.Success(new BaseBooking { BookingId = DateTime.UtcNow.Ticks.ToString(), Provider = Provider }));
    }

    public async Task<Result<List<BaseFlight>>> SearchAsync(SearchRequestDto request)
    {
        try
        {
            var getFlightsResult = await GetFlights(request);
            if (getFlightsResult.IsFailure)
            {
                _logger.LogWarning(
                    "Ошибка при поиске перелётов по запросу {SearchResultKey} {ErrorCode}: {ErrorMessage}",
                    request.SearchResultKey,
                    getFlightsResult.Error.Code,
                    getFlightsResult.Error.Message);

                return Result.Failure<List<BaseFlight>>(getFlightsResult.Error);
            }

            var skyTicketsFlights = getFlightsResult.Value;
            if (!skyTicketsFlights.Any())
            {
                return Result.Failure<List<BaseFlight>>(TicketingProviderErrors.FlightsNotFound(Provider));
            }

            var mapper = new SkyTicketsMapper(Provider, _airlineService, _airportService);

            var baseFlights = new List<BaseFlight>();
            foreach(var flight in skyTicketsFlights)
            {
                var result = await mapper.Map(flight);
                if (result.IsFailure)
                {
                    _logger.LogWarning(
                        "Ошибка маппинга перелёта {TicketingProvider} {ErrorCode}: {ErrorMessage}",
                        Provider.Code,
                        result.Error.Code,
                        result.Error.Message);

                    return Result.Failure<List<BaseFlight>>(result.Error);
                }

                baseFlights.Add(result.Value);
            }

            return baseFlights;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex,
                "Исключение при получении перелётов провайдера {TicketingProvider}",
                Provider.Code);

            return Result.Failure<List<BaseFlight>>(TicketingProviderErrors.SearchFailed(Provider));
        }
    }

    private async Task<Result<List<SkyTicketsFlight>>> GetFlights(SearchRequestDto request)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "TicketingProviders", "SkyTickets", "skytickets-data.json");
        var json = await File.ReadAllTextAsync(jsonPath);
        var flights = JsonSerializer.Deserialize<List<SkyTicketsFlight>>(json);

        if (flights is null)
        {
            return Result.Failure<List<SkyTicketsFlight>>(TicketingProviderErrors.ProviderUnavailable(Provider));
        }

        return flights.Where(f => f.FromAirportCode.Equals(request.From.IATACode.ToUpperInvariant()) &&
                                  f.ToAirportCode.Equals(request.To.IATACode.ToUpperInvariant()) &&
                                  f.Departure.HasValue && (DateOnly.FromDateTime(f.Departure.Value) == request.DepartureDate)).ToList();
    }
}
