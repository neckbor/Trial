using Application.Dictionaries.Airports;
using Application.FlightAggregation;
using Application.FlightSearch;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Primitives;
using Domain.Repositories;
using Domain.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Application.FlightSearch;

public class CreateSearchRequestTests
{
    private readonly ILogger<FlightSearchService> _logger = NullLogger<FlightSearchService>.Instance;
    private readonly Mock<IAirportService> _airportServiceMock = new();
    private readonly Mock<ISearchRequestRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFlightAggregator> _flightAggregatorMock = new();

    private readonly FlightSearchService _service;

    public CreateSearchRequestTests()
    {
        _service = new FlightSearchService(
            _logger,
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _airportServiceMock.Object,
            _flightAggregatorMock.Object);
    }

    [Fact]
    public async Task StartSearch_Should_ReturnSuccess_WhenValidData()
    {
        // Arrange
        var clientId = "";
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
        var from = Airport.DBX;
        var to = Airport.LAX;
        var passengerCount = 1;

        var command = new StartSearchCommand(clientId, departureDate, from.IATACode, to.IATACode, passengerCount);

        _airportServiceMock.Setup(x => x.GetByIATACodeAsync(command.FromAirportIATACode)).ReturnsAsync(Result.Success(from));
        _airportServiceMock.Setup(x => x.GetByIATACodeAsync(command.ToAirportIATACode)).ReturnsAsync(Result.Success(to));
        _repositoryMock.Setup(x => x.InsertAsync(It.IsAny<SearchRequest>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateSearchRequestAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task StartSearch_Should_ReturnError_WhenFromAirportIsNotFound()
    {
        // Arrange
        var command = new StartSearchCommand("", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), "000", "LAX", 1);

        var expectedError = AirportErrors.NotFound;

        _airportServiceMock.Setup(x => x.GetByIATACodeAsync("000"))
            .ReturnsAsync(Result.Failure<Airport>(expectedError));

        // Act
        var result = await _service.CreateSearchRequestAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }


    [Fact]
    public async Task StartSearch_Should_ReturnError_WhenInvalidDepartureDate()
    {
        // Arrange
        var clientId = "";
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10));
        var from = Airport.DBX;
        var to = Airport.LAX;
        var passengerCount = 1;

        var command = new StartSearchCommand(clientId, departureDate, from.IATACode, to.IATACode, passengerCount);

        _airportServiceMock.Setup(x => x.GetByIATACodeAsync(command.FromAirportIATACode)).ReturnsAsync(Result.Success(from));
        _airportServiceMock.Setup(x => x.GetByIATACodeAsync(command.ToAirportIATACode)).ReturnsAsync(Result.Success(to));

        var expectedError = DomainErrors.SearchRequest.InvalidDepartureDate;

        // Act
        var result = await _service.CreateSearchRequestAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }
}
