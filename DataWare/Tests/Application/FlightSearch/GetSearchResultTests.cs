using Application.Dictionaries.Airports;
using Application.FlightAggregation;
using Application.FlightSearch;
using Domain.Primitives;
using Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using FluentAssertions;
using Domain.Entities.Dictionaries;
using Domain.Shared;
using Application.FlightAggregation.DTOs;
using Domain.Models;

namespace Tests.Application.FlightSearch;

public class GetSearchResultTests
{
    private readonly ILogger<FlightSearchService> _logger = NullLogger<FlightSearchService>.Instance;
    private readonly Mock<IAirportService> _airportServiceMock = new();
    private readonly Mock<ISearchRequestRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFlightAggregator> _flightAggregatorMock = new();

    private readonly FlightSearchService _service;

    public GetSearchResultTests()
    {
        _service = new FlightSearchService(
            _logger,
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _airportServiceMock.Object,
            _flightAggregatorMock.Object);
    }

    [Fact]
    public async Task Should_ReturnError_WhenSearchRequest_NotFound()
    {
        // Arrange
        var query = new GetSearchResultsQuery(Guid.Empty);

        _repositoryMock.Setup(r => r.GetByKeyAsync<SearchRequest>(query.SearchRequestId)).ReturnsAsync((SearchRequest)null);

        // Act
        var result = await _service.GetSearchResultAsync(query);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FlightSearchErrors.NotFound);
    }

    [Fact]
    public async Task Should_ReturnError_WhenGetSearchResult_Failed()
    {
        // Arrange
        var sampleSearchRequest = SearchRequest.Create("", Airport.Create(1, "JFK", "JFK"), Airport.Create(2, "LAX", "LAX"), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)), 1).Value;
        var query = new GetSearchResultsQuery(sampleSearchRequest.Id);

        _repositoryMock.Setup(r => r.GetByKeyAsync<SearchRequest>(query.SearchRequestId)).ReturnsAsync(sampleSearchRequest);
        _flightAggregatorMock.Setup(a => a.GetSearchResultAsync(sampleSearchRequest.SearchResultKey)).ReturnsAsync(Result.Failure<SearchResult>(Error.NullValue));

        // Act
        var result = await _service.GetSearchResultAsync(query);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FlightSearchErrors.GetResultFailed);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenGetSearchResult_Successed()
    {
        // Arrange
        var sampleSearchRequest = SearchRequest.Create("", Airport.Create(1, "JFK", "JFK"), Airport.Create(2, "LAX", "LAX"), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)), 1).Value;
        var query = new GetSearchResultsQuery(sampleSearchRequest.Id);

        _repositoryMock.Setup(r => r.GetByKeyAsync<SearchRequest>(query.SearchRequestId)).ReturnsAsync(sampleSearchRequest);
        _flightAggregatorMock.Setup(a => a.GetSearchResultAsync(sampleSearchRequest.SearchResultKey)).ReturnsAsync(SearchResult.Completed(new List<BaseFlight>()));

        // Act
        var result = await _service.GetSearchResultAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
