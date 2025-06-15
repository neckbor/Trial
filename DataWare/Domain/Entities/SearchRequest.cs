using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class SearchRequest : Entity<Guid>
{
    public bool AggregationStarted { get; private set; }
    public string ClientId { get; private set; }

    public long FromAirportId { get; private set; }
    public Airport From { get; private set; }

    public long ToAirportId { get; private set; }
    public Airport To { get; private set; }

    public DateOnly DepartureDate { get; private set; }
    public int PassengerCount { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    public string SearchResultKey { get; private set; }

    private SearchRequest() { }

    private SearchRequest(
        Guid id,
        bool aggregationStarted,
        string clientId,
        Airport from,
        Airport to,
        DateOnly departureDate,
        int passengerCount,
        DateTime createdAtUtc,
        string searchKey)
    {
        Id = id; 
        AggregationStarted = aggregationStarted;
        ClientId = clientId;
        From = from;
        To = to;
        DepartureDate = departureDate;
        PassengerCount = passengerCount;
        CreatedAtUtc = createdAtUtc;
        SearchResultKey = searchKey;
    }

    public static Result<SearchRequest> Create(
        string clientId,
        Airport from,
        Airport to,
        DateOnly departureDate,
        int passengerCount)
    {
        if (from == to)
        {
            return Result.Failure<SearchRequest>(DomainErrors.SearchRequest.SameAirports);
        }

        if (passengerCount <= 0)
        {
            return Result.Failure<SearchRequest>(DomainErrors.SearchRequest.InvalidPassengerCount);
        }

        var now  = DateTime.UtcNow;

        if (departureDate <= DateOnly.FromDateTime(now))
        {
            return Result.Failure<SearchRequest>(DomainErrors.SearchRequest.InvalidDepartureDate);
        }

        string seatchKey = BuildSearchKey(from, to, departureDate);

        return new SearchRequest(Guid.NewGuid(), false, clientId, from, to, departureDate, passengerCount, now, seatchKey);
    }

    private static string BuildSearchKey(Airport from, Airport to, DateOnly departureDate) => $"{from.IATACode}:{to.IATACode}:{departureDate:DDMMYYYY}";

    public void MarkAggregationStarted() => AggregationStarted = true;
}
