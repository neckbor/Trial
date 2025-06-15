using Application.FlightAggregation.DTOs;
using Application.FlightSearch.DTOs;

namespace Application.FlightSearch;

internal static class SearchResultExtensions
{
    public static SearchResult ApplyFiltersAndSorting(this SearchResult searchResult, SearchResultFilterOptions options)
    {
        if (searchResult.Failed) 
        {
            return searchResult;
        }

        var filtered = searchResult.Flights.AsEnumerable();

        if (options.PassengerCount.HasValue)
        {
            filtered = filtered.Where(f => f.AvailableSeats >= options.PassengerCount);
        }

        if (options.MaxPrice.HasValue)
        {
            filtered = filtered.Where(f => f.Fare.TotalPrice <=  options.MaxPrice.Value);
        }

        if (options.MaxTransfers.HasValue) 
        {
            filtered = filtered.Where(f => f.Segments.Count <= options.MaxTransfers.Value + 1); // 1 transfer => 2 segments, 1 segment => 0 transfers
        }

        if (options.AirlineCodes?.Any() == true)
        {
            filtered = filtered.Where(f => f.Segments.All(s => options.AirlineCodes.Contains(s.Airline.IATACode)));
        }

        filtered = options.SortOption switch
        {
            FlightSearchResultSortOption.Price => options.SortDescending
                ? filtered.OrderByDescending(f => f.Fare.TotalPrice)
                : filtered.OrderBy(f => f.Fare.TotalPrice),

            FlightSearchResultSortOption.DepartureTime => options.SortDescending
                ? filtered.OrderByDescending(f => f.DepartureDate)
                : filtered.OrderBy(f => f.DepartureDate),

            FlightSearchResultSortOption.ArrivalTime => options.SortDescending
                ? filtered.OrderByDescending(f => f.ArrivalDate)
                : filtered.OrderBy(f => f.ArrivalDate),

            FlightSearchResultSortOption.Duration => options.SortDescending
                ? filtered.OrderByDescending(f => f.ArrivalDate - f.DepartureDate)
                : filtered.OrderBy(f => f.ArrivalDate - f.DepartureDate),
        };

        return searchResult.WithFlights(filtered.ToList());
    }
}
