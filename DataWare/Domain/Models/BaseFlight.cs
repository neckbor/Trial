using Domain.Entities;

namespace Domain.Models;

public class BaseFlight
{
    public Airport From { get; set; }
    public Airport To { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public Airline Airline { get; set; }
    public FareDetails Fare { get; set; }
    public int AvailableSeats { get; set; }
    public TicketingProvider TicketingProvider { get; set; }
}
