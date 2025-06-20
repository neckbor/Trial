﻿using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using FluentAssertions;
using Infrastructure.TicketingProviders.AirTickets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Infrastructure.TicketingProviders.AirTickets;

public class SearchTests
{
    private readonly ILogger<AIrTicketsTicketingProvider> _logger = NullLogger<AIrTicketsTicketingProvider>.Instance;
    private readonly Mock<IAirlineService> _airlineService = new();
    private readonly Mock<IAirportService> _airportService = new();

    private readonly AIrTicketsTicketingProvider _provider;

    public SearchTests()
    {
        _provider = new AIrTicketsTicketingProvider(
            _logger,
            _airlineService.Object,
            _airportService.Object);
    }

    [Fact(Skip = "Боевой запуск")]
    public async Task Debug_Test()
    {
        // Arrange
        var clientId = "";
        var from = Airport.LAX;
        var to = Airport.CDG;
        var departureDate = new DateOnly(2025, 7, 1);
        var passengerCount = 1;

        var searchRequestDto = new SearchRequestDto
        {
            SearchResultKey = "sample-key",
            From = new AirportDto { Id = from.Id, IATACode = from.IATACode },
            To = new AirportDto { Id = to.Id, IATACode = to.IATACode },
            DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3))
        };

        var airline = Airline.AirFrance;

        _airlineService.Setup(s => s.GetByICAOCodeAsync(It.IsAny<string>())).ReturnsAsync(airline);
        _airportService.Setup(s => s.GetByIATACodeAsync("LAX")).ReturnsAsync(from);
        _airportService.Setup(s => s.GetByIATACodeAsync("CDG")).ReturnsAsync(to);

        // Act 
        var result = await _provider.SearchAsync(searchRequestDto);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
