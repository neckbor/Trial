using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;

namespace Domain.Entities;

public class FlightSegment
{
    public Flight Flight { get; private set; }
    public Airline Airline { get; private set; }
    public Airport From { get; private set; }
    public Airport To { get; private set; }
    public DateTime DepartureDateUtc { get; private set; }
    public DateTime ArrivalDateUtc { get; private set; }

    private FlightSegment(
        Flight flight,
        Airline airline,
        Airport from,
        Airport to,
        DateTime departureDateUtc,
        DateTime arrivalDateUtc)
    {
        Flight = flight;
        Airline = airline;
        From = from;
        To = to;
        DepartureDateUtc = departureDateUtc;
        ArrivalDateUtc = arrivalDateUtc;
    }

    internal static Result<FlightSegment> Create(Flight flight, BaseSegment segmentModel)
    {
        if (segmentModel.DepartureDateUtc >= segmentModel.ArrivalDateUtc) 
        {
            return Result.Failure<FlightSegment>(DomainErrors.FlightSegment.InvalidSegmentDates);
        }

        return new FlightSegment(
            flight, 
            segmentModel.Airline, 
            segmentModel.From, 
            segmentModel.To, 
            segmentModel.DepartureDateUtc, 
            segmentModel.ArrivalDateUtc);
    }
}
