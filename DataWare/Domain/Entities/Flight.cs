using Domain.Models;

namespace Domain.Entities;

public class Flight
{
    private readonly List<FlightSegment> _flightSegments = [];

    public Airport From { get; private set; }
    public Airport To { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public TicketingProvider TicketingProvider { get; private set; }
    public decimal TotalPrice { get; private set; }

    public IReadOnlyCollection<FlightSegment> Segments => _flightSegments.AsReadOnly();

    private Flight() { }

    private Flight(
        Airport from,
        Airport to,
        DateTime departureDate,
        DateTime arrivalDate,
        TicketingProvider ticketingProvider,
        decimal totalPrice)
    {
        From = from;
        To = to;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        TicketingProvider = ticketingProvider;
        TotalPrice = totalPrice;
    }

    internal static Flight Create(BaseFlight flightModel)
    {
        var flight = new Flight(
            flightModel.From,
            flightModel.To,
            flightModel.DepartureDate,
            flightModel.ArrivalDate,
            flightModel.TicketingProvider,
            flightModel.Fare.TotalPrice);

        foreach (var segmentModel in flightModel.Segments) 
        {
            flight.AddSegment(segmentModel);
        }

        return flight;
    }

    private void AddSegment(BaseSegment segment)
    {
        _flightSegments.Add(FlightSegment.Create(this, segment));
    }
}
