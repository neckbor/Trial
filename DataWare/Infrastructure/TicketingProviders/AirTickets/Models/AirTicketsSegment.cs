namespace Infrastructure.TicketingProviders.AirTickets.Models;

internal class AirTicketsSegment
{
    public string AirlineCode { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime DepartureUtc { get; set; }
    public DateTime ArrivalUtc { get; set; }
    public int AvailableSeats { get; set; }
}
