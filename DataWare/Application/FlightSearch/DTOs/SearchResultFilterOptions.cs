﻿namespace Application.FlightSearch.DTOs;

public class SearchResultFilterOptions
{
    public decimal? MaxPrice { get; set; }
    public int? MaxTransfers { get; set; }
    public List<string>? AirlineCodes { get; set; }

    public int? PassengerCount { get; set; }

    public FlightSearchResultSortOption SortOption { get; set; }
    public bool SortDescending { get; set; } = false;

    public void WithPassengerCount(int passengerCount) => PassengerCount = passengerCount;
}

