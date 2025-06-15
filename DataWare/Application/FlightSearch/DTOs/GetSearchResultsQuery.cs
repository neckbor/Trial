namespace Application.FlightSearch.DTOs;

public record GetSearchResultsQuery(Guid SearchRequestId, SearchResultFilterOptions FilterOptions);
