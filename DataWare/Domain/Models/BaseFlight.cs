using Domain.Entities.Dictionaries;

namespace Domain.Models;

public class BaseFlight
{
    public TicketingProvider TicketingProvider { get; set; }
    public string ProviderFlightId
    {
        get
        {
            if (string.IsNullOrEmpty(ProviderFlightId))
            {
                return $"{From.IATACode}{To.IATACode}{DepartureDate:ddMMYYYY}";
            }

            return ProviderFlightId;
        }

        set { }
    }
    public Airport From 
    {
        get
        {
            return Segments.OrderBy(s => s.DepartureDateUtc).Select(s => s.From).First();
        }
        private set { }
    }

    public Airport To 
    {
        get
        {
            return Segments.OrderByDescending(s => s.ArrivalDateUtc).Select(s => s.To).First();
        }
        private set { } 
    }

    public DateTime DepartureDate 
    {
        get
        {
            return Segments.Min(s => s.DepartureDateUtc);
        }
        private set { }
    }

    public DateTime ArrivalDate 
    {
        get
        {
            return Segments.Max(s => s.ArrivalDateUtc);
        }
        private set { }
    }

    public int AvailableSeats 
    {
        get
        {
            return Segments.Min(s => s.AvailableSeats);
        }
        private set { }
    }

    public FareDetails Fare { get; set; }

    public List<BaseSegment> Segments { get; set; }
}
