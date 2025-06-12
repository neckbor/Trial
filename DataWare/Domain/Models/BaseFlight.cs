using Domain.Entities.Dictionaries;

namespace Domain.Models;

public class BaseFlight
{
    public TicketingProvider TicketingProvider { get; set; }
    public Airport From { get; set; }
    public Airport To { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public FareDetails Fare { get; set; }
    public int AvailableSeats { get; set; }
    public List<BaseSegment> Segments { get; set; }
}
