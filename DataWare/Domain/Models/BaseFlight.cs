using Domain.Entities.Dictionaries;

namespace Domain.Models;

public class BaseFlight
{
    private string _flightId = "";

    public TicketingProvider TicketingProvider { get; set; }
    public string FlightId
    {
        get
        {
            if (string.IsNullOrEmpty(_flightId))
            {
                _flightId = $"{TicketingProvider.Code}:{string.Join(":", Segments.Select(s => s.FlightNumber))}";
            }

            return _flightId;
        }

        set 
        {
            _flightId = value;
        }
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
