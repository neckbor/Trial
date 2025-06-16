using Application.FlightAggregation;
using Application.InfrastructureAbstractions;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Application.FlightAggregation;

public class GetSearchResultTests
{
    private readonly ILogger<FlightAggregator> _logger = NullLogger<FlightAggregator>.Instance;
    private readonly Mock<ISearchResultCache> _cacheMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly FlightAggregator _flightAggregator;

    public GetSearchResultTests()
    {
        _flightAggregator = new FlightAggregator(
            _logger,
            _cacheMock.Object,
            new List<ITicketingProvider>(),
            _scopeFactoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnFail_WhenGetFLightsResult_Failed()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Failure<List<BaseFlight>>(Error.NullValue));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Failed);
    }

    [Fact]
    public async Task Should_ReturnFail_WhenGetProviderSearchStatus_Failed()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(new List<BaseFlight>()));

        _cacheMock.Setup(c => c.GetProviderSearchStatusAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Failure<Dictionary<TicketingProvider, SearchStatus>>(Error.NullValue));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Failed);
    }

    [Fact]
    public async Task Should_ReturnPending_WhenProviderSearchStatuses_IsEmpty()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(new List<BaseFlight>()));

        _cacheMock.Setup(c => c.GetProviderSearchStatusAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(new Dictionary<TicketingProvider, SearchStatus>()));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Pending);
    }

    [Fact]
    public async Task Should_ReturPending_WhenAnyProvider_Pending()
    {
        // Arrange
        var sampleProvider = TicketingProvider.AirTickets;
        Dictionary<TicketingProvider, SearchStatus> providerSearchStatuses = new Dictionary<TicketingProvider, SearchStatus>()
        {
            { sampleProvider, SearchStatus.Pending }
        };

        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(new List<BaseFlight>()));

        _cacheMock.Setup(c => c.GetProviderSearchStatusAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(providerSearchStatuses));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Pending);
    }

    [Fact]
    public async Task Should_ReturnFlightsNotFoundError_WhenFligthEmpty_AndNoPendingProviderStatuses()
    {
        // Arrange
        var sampleProvider = TicketingProvider.AirTickets;
        Dictionary<TicketingProvider, SearchStatus> providerSearchStatuses = new Dictionary<TicketingProvider, SearchStatus>()
        {
            { sampleProvider, SearchStatus.Completed }
        };

        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(new List<BaseFlight>()));

        _cacheMock.Setup(c => c.GetProviderSearchStatusAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(providerSearchStatuses));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Failed);
        result.Value.Error.Should().Be(FlightAggregationErrors.FlightsNotFound);
    }

    [Fact]
    public async Task Should_ReturnCompleted_WhenNoProvidersPending_AndFlightsIsNotEmpty()
    {
        // Arrange
        var sampleProvider = TicketingProvider.AirTickets;
        Dictionary<TicketingProvider, SearchStatus> providerSearchStatuses = new Dictionary<TicketingProvider, SearchStatus>()
        {
            { sampleProvider, SearchStatus.Completed }
        };

        var sampleFlight = new BaseFlight();
        List<BaseFlight> flights = new List<BaseFlight>()
        {
            sampleFlight
        };

        _cacheMock.Setup(c => c.GetFlightsAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(flights));

        _cacheMock.Setup(c => c.GetProviderSearchStatusAsync(It.IsAny<string>()))
                  .ReturnsAsync(Result.Success(providerSearchStatuses));

        // Act
        var result = await _flightAggregator.GetSearchResultAsync("search:sample");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(SearchStatus.Completed);
    }
}
