namespace Infrastructure.TicketingProviders.SkyTickets.Models;

internal class SkyTicketsFlightLeg
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }

    public string DepUtc { get; set; } // ISO 8601 string
    public string ArrUtc { get; set; }

    public int AvailableSeats { get; set; }
}
