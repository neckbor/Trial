using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class Flight : Entity<Guid>
{
    private readonly List<FlightSegment> _flightSegments = [];

    public Booking Booking { get; private set; }

    public long FromAirportId { get; private set; }
    public Airport From { get; private set; }

    public long ToAirportId { get; private set; }
    public Airport To { get; private set; }

    public DateTime DepartureDate { get; private set; }
    public DateTime ArrivalDate { get; private set; }

    public int TicketingProviderId { get; private set; }
    public TicketingProvider TicketingProvider { get; private set; }
    public decimal TotalPrice { get; private set; }

    public IReadOnlyCollection<FlightSegment> Segments => _flightSegments.AsReadOnly();

    private Flight() { }

    private Flight(
        Booking booking,
        Airport from,
        Airport to,
        DateTime departureDate,
        DateTime arrivalDate,
        TicketingProvider ticketingProvider,
        decimal totalPrice)
    {
        Booking = booking;
        From = from;
        To = to;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        TicketingProvider = ticketingProvider;
        TotalPrice = totalPrice;
    }

    internal static Result<Flight> Create(Booking booking, BaseFlight flightModel)
    {
        var flight = new Flight(
            booking,
            flightModel.From,
            flightModel.To,
            flightModel.DepartureDate,
            flightModel.ArrivalDate,
            flightModel.TicketingProvider,
            flightModel.Fare.TotalPrice);

        foreach (var segmentModel in flightModel.Segments) 
        {
            var createFlightSegmentResult = flight.AddSegment(segmentModel);
            if (createFlightSegmentResult.IsFailure)
            {
                return Result.Failure<Flight>(createFlightSegmentResult.Error);
            }
        }

        return flight;
    }

    private Result AddSegment(BaseSegment segment)
    {
        var createFlightSegmentResult = FlightSegment.Create(this, segment);
        if (createFlightSegmentResult.IsFailure)
        {
            return Result.Failure(createFlightSegmentResult.Error);
        }

        _flightSegments.Add(createFlightSegmentResult.Value);

        return Result.Success();
    }
}
