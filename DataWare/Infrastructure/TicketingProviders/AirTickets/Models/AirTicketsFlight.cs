namespace Infrastructure.TicketingProviders.AirTickets.Models;

internal class AirTicketsFlight
{
    public string Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Arrival { get; set; }
    public decimal Price { get; set; }
    public int Seats { get; set; }
    public List<AirTicketsSegment> Segments { get; set; }
}
