using Domain.Entities.Dictionaries;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Cache;
using Infrastructure.Cache.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Infrastructure.Cache;

public class MemoryCacheTests
{
    private readonly ILogger<SearchResultMemoryCache> _logger = NullLogger<SearchResultMemoryCache>.Instance;
    private readonly SearchResultMemoryCache _cache;

    public MemoryCacheTests()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cache = new SearchResultMemoryCache(_logger, memoryCache);
    }

    [Fact]
    public async Task AddFlightsAsync_ThenGetFlights_ReturnsSameFlights()
    {
        // Arrange
        var searchKey = "test-key-1";
        var providerFlightId = Guid.NewGuid().ToString();
        var flightNumber = "AAA1";

        var flights = new List<BaseFlight>
        {
            new()
            {
                TicketingProvider = TicketingProvider.AirTickets,
                ProviderFlightId = providerFlightId,
                Fare = new FareDetails { BaseFare = 100, Taxes = 20 },
                Segments = new List<BaseSegment>()
                {
                    new()
                    {
                        FlightNumber = flightNumber,
                        Airline = Airline.AirFrance,
                        From = Airport.CDG,
                        To = Airport.LAX,
                        DepartureDateUtc = DateTime.UtcNow,
                        ArrivalDateUtc = DateTime.UtcNow.AddHours(1),
                        AvailableSeats = 3
                    }
                }
            }
        };

        // Act
        await _cache.AddFlightsAsync(searchKey, flights);
        var result = await _cache.GetFlightsAsync(searchKey);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value[0].ProviderFlightId.Should().Be(providerFlightId);
    }

    [Fact]
    public async Task AddFlightsAsync_AppendsToExistingFlights()
    {
        // Arrange
        var searchKey = "test-key-2";

        var firstBatch = new List<BaseFlight>
        {
            new()
            {
                TicketingProvider = TicketingProvider.AirTickets,
                ProviderFlightId = Guid.NewGuid().ToString(),
                Fare = new FareDetails { BaseFare = 100, Taxes = 20 },
                Segments = new List<BaseSegment>()
                {
                    new()
                    {
                        FlightNumber = "AAA1",
                        Airline = Airline.AirFrance,
                        From = Airport.CDG,
                        To = Airport.LAX,
                        DepartureDateUtc = DateTime.UtcNow,
                        ArrivalDateUtc = DateTime.UtcNow.AddHours(1),
                        AvailableSeats = 3
                    }
                }
            }
        };

        var secondBatch = new List<BaseFlight>
        {
            new()
            {
                TicketingProvider = TicketingProvider.AirTickets,
                ProviderFlightId = Guid.NewGuid().ToString(),
                Fare = new FareDetails { BaseFare = 200, Taxes = 0 },
                Segments = new List<BaseSegment>()
                {
                    new()
                    {
                        FlightNumber = "BBB2",
                        Airline = Airline.Emirates,
                        From = Airport.DBX,
                        To = Airport.TBS,
                        DepartureDateUtc = DateTime.UtcNow.AddHours(2),
                        ArrivalDateUtc = DateTime.UtcNow.AddHours(5),
                        AvailableSeats = 1
                    }
                }
            }
        };

        // Act
        await _cache.AddFlightsAsync(searchKey, firstBatch);
        await _cache.AddFlightsAsync(searchKey, secondBatch);

        var result = await _cache.GetFlightsAsync(searchKey);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(2);
        result.Value[1].To.Should().Be(Airport.TBS);
    }

    [Fact]
    public async Task GetFlightsAsync_WhenNotExists_ReturnsError()
    {
        // Arrange
        var searchKey = "missing-key";

        // Act
        var result = await _cache.GetFlightsAsync(searchKey);

        // Arrange
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Should().Be(CacheErrors.Expired);
    }

    [Fact]
    public async Task SetAndGetProviderSearchStatus_WorksCorrectly()
    {
        // Arrange
        var key = "status-key";

        // Act
        await _cache.SetProviderSearchStatusAsync(key, TicketingProvider.AirTickets, SearchStatus.Completed);

        var result = await _cache.GetProviderSearchStatusAsync(key);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TryGetValue(TicketingProvider.AirTickets, out var status).Should().BeTrue();
        status.Should().Be(SearchStatus.Completed);
    }

    [Fact]
    public async Task CLearAsync_RemovesCachedValues()
    {
        // Arrange
        var key = "clear-key";

        await _cache.AddFlightsAsync(key, new List<BaseFlight>());
        await _cache.SetProviderSearchStatusAsync(key, TicketingProvider.AirTickets, SearchStatus.Completed);

        // Act
        await _cache.CLearAsync(key);

        var flights = await _cache.GetFlightsAsync(key);
        var statuses = await _cache.GetProviderSearchStatusAsync(key);

        // Assert
        flights.IsFailure.Should().BeTrue();
        flights.Error.Should().Be(CacheErrors.Expired);

        statuses.IsFailure.Should().BeTrue();
        statuses.Error.Should().Be(CacheErrors.Expired);
    }
}
