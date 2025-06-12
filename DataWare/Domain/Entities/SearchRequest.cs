using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class SearchRequest : Entity<Guid>
{
    public SearchStatus Status { get; private set; }
    public string ClientId { get; private set; }
    public Airport From { get; private set; }
    public Airport To { get; private set; }
    public DateOnly DepartureDate { get; private set; }
    public int PassengerCount { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    private SearchRequest(
        Guid id,
        SearchStatus status,
        string clientId,
        Airport from,
        Airport to,
        DateOnly departureDate,
        int passengerCount,
        DateTime createdAtUtc)
    {
        Id = id; 
        Status = status;
        ClientId = clientId;
        From = from;
        To = to;
        DepartureDate = departureDate;
        PassengerCount = passengerCount;
        CreatedAtUtc = createdAtUtc;
    }

    public static Result<SearchRequest> Create(
        string clientId,
        Airport from,
        Airport to,
        DateOnly departureDate,
        int passengerCount)
    {
        var now  = DateTime.UtcNow;

        if (departureDate <= DateOnly.FromDateTime(now))
        {
            return Result.Failure<SearchRequest>(DomainErrors.SearchRequest.InvalidDepartureDate);
        }

        return new SearchRequest(Guid.NewGuid(), SearchStatus.Pending, clientId, from, to, departureDate, passengerCount, now);
    }
}
