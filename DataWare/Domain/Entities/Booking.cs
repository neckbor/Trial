using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Models;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class Booking : Entity<Guid>
{
    private readonly List<Passenger> _passengers = [];

    public string ClientId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public int StatusId { get; private set; }
    public BookingStatus Status { get; private set; }

    public int TicketingProviderId { get; private set; }
    public TicketingProvider TicketingProvider { get; private set; }

    public string ExternalBookingId { get; private set; }
    public Flight Flight { get; private set; }
    public IReadOnlyCollection<Passenger> Passengers => _passengers.AsReadOnly();

    private Booking() { }

    private Booking(
        Guid id,
        TicketingProvider ticketingProvider,
        string externalBookingId,
        List<Passenger> passengers,
        DateTime createdAt,
        BookingStatus status,
        string clientId) 
    {
        Id = id;
        TicketingProviderId = ticketingProvider.Id;
        ExternalBookingId = externalBookingId;
        _passengers = passengers;
        CreatedAt = createdAt;
        StatusId = status.Id;
        ClientId = clientId;
    }

    public static Result<Booking> Create(
        TicketingProvider ticketetingProvider,
        string externalBookingId,
        BaseFlight baseFlight,
        List<Passenger> passengers,
        string clientId)
    {
        var now = DateTime.UtcNow;

        var booking = new Booking(
            Guid.NewGuid(),
            ticketetingProvider,
            externalBookingId,
            passengers,
            now,
            BookingStatus.Booked,
            clientId);

        var createFlightResult = booking.AddFlight(baseFlight);
        if (createFlightResult.IsFailure)
        {
            return Result.Failure<Booking>(createFlightResult.Error);
        }

        return booking;
    }

    private Result AddFlight(BaseFlight flightModel)
    {
        if (Flight is not null)
        {
            return Result.Failure(DomainErrors.Booking.AlreadyHasFlight);
        }

        var createFlightResult = Flight.Create(this, flightModel);
        if (createFlightResult.IsFailure) 
        {
            return Result.Failure(createFlightResult.Error);
        }

        Flight = createFlightResult.Value;

        return Result.Success();
    }
}
