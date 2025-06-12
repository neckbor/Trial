using Domain.Models;

namespace Domain.Entities;

public class Booking
{
    private readonly List<Flight> _flights = [];
    private readonly List<Passenger> _passengers = [];

    public Guid Id { get; set; }
    public string ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public BookingStatus Status { get; set; }
    public decimal TotalPrice => _flights.Sum(f => f.TotalPrice);
    public IReadOnlyCollection<Flight> Flights => _flights.AsReadOnly();
    public IReadOnlyCollection<Passenger> Passengers => _passengers.AsReadOnly();

    private Booking(
        Guid id,
        List<Passenger> passengers,
        DateTime createdAt,
        BookingStatus status,
        string clientId) 
    {
        _passengers = passengers;
        CreatedAt = createdAt;
        Status = status;
        ClientId = clientId;
    }

    public static Booking Create(
        List<BaseFlight> flightData,
        List<Passenger> passengers,
        string clientId)
    {
        var now = DateTime.UtcNow;

        var booking = new Booking(
            Guid.NewGuid(),
            passengers,
            now,
            BookingStatus.Created,
            clientId);

        foreach (var flight in flightData) 
        {
            booking.AddFlight(flight);
        }

        return booking;
    }

    private void AddFlight(BaseFlight flightModel)
    {
        _flights.Add(Flight.Create(this, flightModel));
    }
}
