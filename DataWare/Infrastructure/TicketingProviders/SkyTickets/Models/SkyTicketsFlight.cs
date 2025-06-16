namespace Infrastructure.TicketingProviders.SkyTickets.Models;

internal class SkyTicketsFlight
{
    public Guid Id { get; set; }
    public string FromAirportCode { get; set; }
    public string ToAirportCode { get; set; }

    public DateTime? Departure
    {
        get
        {
            if (DateTime.TryParse(DepartureIso, out var dtime))
            {
                return dtime;
            }

            return null;
        }
        private set { }
    }

    public string DepartureIso { get; set; }

    public DateTime? Arrival
    {
        get
        {
            if (DateTime.TryParse(ArrivalIso, out var dtime))
            {
                return dtime;
            }

            return null;
        }
        private set { }
    }

    public string ArrivalIso { get; set; }

    public double Price { get; set; }
    public int Seats { get; set; }

    public List<SkyTicketsFlightLeg> Legs { get; set; }
}
