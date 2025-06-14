using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Application.FlightAggregation.DTOs;

public class SearchResult
{
    public List<BaseFlight> Flights { get; private set; } = [];
    public SearchStatus Status { get; private set; }
    public Error? Error { get; private set; }

    private SearchResult(List<BaseFlight>? flights, SearchStatus status, Error? error)
    {
        Flights = flights ?? [];
        Status = status;
        Error = error;
    }
    
    public static SearchResult Fail(Error error)
    {
        return new SearchResult(null, SearchStatus.Failed, error);
    }

    public static SearchResult Pending(List<BaseFlight> flights)
    {
        return new SearchResult(flights, SearchStatus.Pending, null);
    }

    public static SearchResult Completed(List<BaseFlight> flights)
    {
        return new SearchResult(flights, SearchStatus.Completed, null);
    }
}
