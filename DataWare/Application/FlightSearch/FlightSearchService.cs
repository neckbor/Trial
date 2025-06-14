using Application.Dictionaries.Airports;
using Application.Errors;
using Application.FlightAggregation;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Primitives;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.FlightSearch;

internal class FlightSearchService : IFlightSearchService
{
    private readonly ILogger<FlightSearchService> _logger;

    private readonly ISearchRequestRepository _searchRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IAirportService _airportService;
    private readonly IFlightAggregator _flightAggregator;

    public FlightSearchService(
        ILogger<FlightSearchService> logger,
        ISearchRequestRepository searchRequestRepository, 
        IUnitOfWork unitOfWork, 
        IAirportService airportService, 
        IFlightAggregator flightAggregator)
    {
        _searchRequestRepository = searchRequestRepository;
        _airportService = airportService;
        _unitOfWork = unitOfWork;
        _flightAggregator = flightAggregator;
        _logger = logger;
    }

    public async Task<Result<string>> CreateSearchRequestAsync(StartSearchCommand command)
    {
        var getFromAirportResult = await _airportService.GetByIATACodeAsync(command.FromAirportIATACode);
        if (getFromAirportResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при получении аэропорта отправления {ErrorCode}: {ErrorMessage}", getFromAirportResult.Error.Code, getFromAirportResult.Error.Message);

            return Result.Failure<string>(getFromAirportResult.Error);
        }
        var fromAirport = getFromAirportResult.Value;

        var getToAirportResult = await _airportService.GetByIATACodeAsync(command.ToAirportIATACode);
        if (getToAirportResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при получении аэропорта прибытия {ErrorCode}: {ErrorMessage}", getToAirportResult.Error.Code, getToAirportResult.Error.Message);

            return Result.Failure<string>(getToAirportResult.Error);
        }
        var toAirport = getToAirportResult.Value;

        var createSearchRequestResult = SearchRequest.Create(command.ClientId, fromAirport, toAirport, command.DepartureDateUtc, command.PassengerCount);
        if (createSearchRequestResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при создании запроса поиска {ErrorCode}: {ErrorMessage}", createSearchRequestResult.Error.Code, createSearchRequestResult.Error.Message);

            return Result.Failure<string>(createSearchRequestResult.Error);
        }
        var searchRequest = createSearchRequestResult.Value;

        try
        {
            await _searchRequestRepository.InsertAsync(searchRequest);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Создан запрос н поиск {SearchRequestId}", searchRequest.Id);

            return searchRequest.SearchResultKey;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Исключение при сохранении нового запроса поиска по команде {@Command}", command);

            return Result.Failure<string>(ApplicationErrors.General.Unexpected);
        }
    }

}
