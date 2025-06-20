﻿using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Models;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class FlightSegment : Entity<Guid>
{
    public Guid FlightId { get; private set; }
    public Flight Flight { get; private set; }
    public string FlightNumber { get; private set; }

    public int AirlineId { get; private set; }
    public Airline Airline { get; private set; }

    public long FromAirportId { get; private set; }
    public Airport From { get; private set; }

    public long ToAirportId { get; private set; }
    public Airport To { get; private set; }

    public DateTime DepartureDateUtc { get; private set; }
    public DateTime ArrivalDateUtc { get; private set; }

    private FlightSegment() { }

    private FlightSegment(
        Flight flight,
        string flightNumber,
        Airline airline,
        Airport from,
        Airport to,
        DateTime departureDateUtc,
        DateTime arrivalDateUtc)
    {
        Flight = flight;
        FlightNumber = flightNumber;
        AirlineId = airline.Id;
        FromAirportId = from.Id;
        ToAirportId = to.Id;
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
            segmentModel.FlightNumber,
            segmentModel.Airline, 
            segmentModel.From, 
            segmentModel.To, 
            segmentModel.DepartureDateUtc, 
            segmentModel.ArrivalDateUtc);
    }
}
