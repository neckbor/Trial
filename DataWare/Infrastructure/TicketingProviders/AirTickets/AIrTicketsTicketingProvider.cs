using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Application.InfrastructureAbstractions;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;
using Infrastructure.TicketingProviders.AirTickets.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.TicketingProviders.AirTickets;

internal class AIrTicketsTicketingProvider : ITicketingProvider
{
    private readonly ILogger<AIrTicketsTicketingProvider> _logger;
    private readonly IAirlineService _airlineService;
    private readonly IAirportService _airportService;

    public TicketingProvider Provider => TicketingProvider.AirTickets;

    public AIrTicketsTicketingProvider(ILogger<AIrTicketsTicketingProvider> logger, IAirlineService airlineService, IAirportService airportService)
    {
        _logger = logger;
        _airlineService = airlineService;
        _airportService = airportService;
    }

    public async Task<Result<List<BaseFlight>>> SearchAsync(SearchRequest request)
    {
        try
        {
            var getFlightsResult = await GetFlights(request);
            if (getFlightsResult.IsFailure)
            {
                _logger.LogWarning(
                    "Ошибка при поиске перелётов по запросу {SearchRequestId} {ErrorCode}: {ErrorMessage}",
                    request.Id,
                    getFlightsResult.Error.Code,
                    getFlightsResult.Error.Message);

                return Result.Failure<List<BaseFlight>>(getFlightsResult.Error);
            }

            var airTicketsFlights = getFlightsResult.Value;
            if (!airTicketsFlights.Any())
            {
                return Result.Failure<List<BaseFlight>>(TicketingProviderErrors.FlightsNotFound(Provider));
            }

            var mapper = new AirTicketsMapper(Provider, _airlineService, _airportService);

            var baseFlights = new List<BaseFlight>();
            foreach (var flight in airTicketsFlights)
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

            return Result.Success(baseFlights);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex,
                "Исключение при получении перелётов провайдера {TicketingProvider}",
                Provider.Code);

            return Result.Failure<List<BaseFlight>>(TicketingProviderErrors.SearchFailed(Provider));
        }
    }

    private async Task<Result<List<AirTicketsFlight>>> GetFlights(SearchRequest request)
    {
        var jsonPath = Path.Combine("TicketingProviders", "AirTickets", "airtickets-data.json");
        var json = await File.ReadAllTextAsync(jsonPath);
        var flights = JsonSerializer.Deserialize<List<AirTicketsFlight>>(json);

        if (flights == null)
        {
            return Result.Failure<List<AirTicketsFlight>>(TicketingProviderErrors.ProviderUnavailable(Provider));
        }

        return flights.Where(f => f.From.Equals(request.From.IATACode.ToUpperInvariant()) &&
                                  f.To.Equals(request.To.IATACode.ToUpperInvariant()) &&
                                  DateOnly.FromDateTime(f.Departure) == request.DepartureDate).ToList();
    }

    public Task<Result<BaseBooking>> BookAsync(string flightId, List<Passenger> passengers)
    {
        return Task.FromResult(Result.Success(new BaseBooking { Provider = Provider, BookingId = Guid.NewGuid().ToString() }));
    }
}
