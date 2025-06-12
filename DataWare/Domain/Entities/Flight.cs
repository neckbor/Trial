using Domain.Models;

namespace Domain.Entities;

public class Flight
{
    public Airport From { get; private set; }
    public Airport To { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public Airline Airline { get; private set; }
    public TicketingProvider TicketingProvider { get; private set; }
    public decimal TotalPrice { get; private set; }

    private Flight() { }

    private Flight(
        Airport from,
        Airport to,
        DateTime departureDate,
        DateTime arrivalDate,
        Airline airline,
        TicketingProvider ticketingProvider,
        decimal totalPrice)
    {
        From = from;
        To = to;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        Airline = airline;
        TicketingProvider = ticketingProvider;
        TotalPrice = totalPrice;
    }

    internal static Flight Create(BaseFlight flightModel)
    {
        return new(
            flightModel.From,
            flightModel.To,
            flightModel.DepartureDate,
            flightModel.ArrivalDate,
            flightModel.Airline,
            flightModel.TicketingProvider,
            flightModel.Fare.TotalPrice);
    }
}
