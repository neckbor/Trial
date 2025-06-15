using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Errors;
using FluentAssertions;

namespace Tests.Domain.Entities;

public class SearchRequestTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenValidData()
    {
        // Arrange
        var clientId = "";
        var from = Airport.DBX;
        var to = Airport.LAX;
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
        var passengerCount = 1;

        // Act
        var result = SearchRequest.Create(clientId, from, to, departureDate, passengerCount);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(null);
    }

    [Fact]
    public void Create_Should_ReturnError_WhenInvalidDate()
    {
        // Arrange
        var clientId = "";
        var from = Airport.DBX;
        var to = Airport.LAX;
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var passengerCount = 1;

        // Act
        var result = SearchRequest.Create(clientId, from, to, departureDate, passengerCount);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.SearchRequest.InvalidDepartureDate);
    }

    [Fact]
    public void Create_Should_ReturnError_WhenSameAirports()
    {
        // Arrange
        var clientId = "";
        var from = Airport.LAX;
        var to = from;
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
        var passengerCount = 1;

        // Act
        var result = SearchRequest.Create(clientId, from, to, departureDate, passengerCount);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.SearchRequest.SameAirports);
    }

    [Fact]
    public void Create_Should_ReturnError_WhenInvalidPassengerCount()
    {
        // Arrange
        var clientId = "";
        var from = Airport.DBX;
        var to = Airport.LAX; ;
        var departureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
        var passengerCount = 0;

        // Act
        var result = SearchRequest.Create(clientId, from, to, departureDate, passengerCount);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.SearchRequest.InvalidPassengerCount);
    }
}
