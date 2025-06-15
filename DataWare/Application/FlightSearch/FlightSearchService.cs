using Application.Dictionaries.Airports;
using Application.Errors;
using Application.FlightAggregation;
using Application.FlightAggregation.DTOs;
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

    public async Task<Result<Guid>> CreateSearchRequestAsync(StartSearchCommand command)
    {
        var getFromAirportResult = await _airportService.GetByIATACodeAsync(command.FromAirportIATACode);
        if (getFromAirportResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при получении аэропорта отправления {ErrorCode}: {ErrorMessage}", getFromAirportResult.Error.Code, getFromAirportResult.Error.Message);

            return Result.Failure<Guid>(getFromAirportResult.Error);
        }
        var fromAirport = getFromAirportResult.Value;

        var getToAirportResult = await _airportService.GetByIATACodeAsync(command.ToAirportIATACode);
        if (getToAirportResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при получении аэропорта прибытия {ErrorCode}: {ErrorMessage}", getToAirportResult.Error.Code, getToAirportResult.Error.Message);

            return Result.Failure<Guid>(getToAirportResult.Error);
        }
        var toAirport = getToAirportResult.Value;

        var createSearchRequestResult = SearchRequest.Create(command.ClientId, fromAirport, toAirport, command.DepartureDateUtc, command.PassengerCount);
        if (createSearchRequestResult.IsFailure) 
        {
            _logger.LogWarning("Ошибка при создании запроса поиска {ErrorCode}: {ErrorMessage}", createSearchRequestResult.Error.Code, createSearchRequestResult.Error.Message);

            return Result.Failure<Guid>(createSearchRequestResult.Error);
        }
        var searchRequest = createSearchRequestResult.Value;

        try
        {
            await _searchRequestRepository.InsertAsync(searchRequest);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Создан запрос н поиск {SearchRequestId}", searchRequest.Id);

            return searchRequest.Id;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Исключение при сохранении нового запроса поиска по команде {@Command}", command);

            return Result.Failure<Guid>(ApplicationErrors.General.Unexpected);
        }
    }

    public async Task<Result> LaunchFlightSearchAsync()
    {
        var unprocessed = await _searchRequestRepository.SearchAsync<SearchRequest>(r => !r.AggregationStarted);

        var searchGroups = unprocessed.GroupBy(s => s.SearchResultKey);
        foreach (var group in searchGroups)
        {
            string searchKey = group.Key;

            await _flightAggregator.AggregateAsync(group.First());

            foreach (var request in group)
            {
                request.MarkAggregationStarted();
            }
        }

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Исключение при сохранении запросов на поиск");
        }

        return Result.Success();
    }

    public async Task<Result<SearchResult>> GetSearchResultAsync(GetSearchResultsQuery query)
    {
        var request = await _searchRequestRepository.GetByKeyAsync<SearchRequest>(query.SearchRequestId);
        if (request is null)
        {
            return Result.Failure<SearchResult>(FlightSearchErrors.NotFound);
        }

        var getSearchResultResult = await _flightAggregator.GetSearchResultAsync(request.SearchResultKey);
        if (getSearchResultResult.IsFailure) 
        {
            _logger.LogWarning(
                "При получении результатов поиска {SearchRequestId} по ключу {SearchKey} произошла ошибка {ErrorCode}: {ErrorMessage}",
                request.Id,
                request.SearchResultKey,
                getSearchResultResult.Error.Code,
                getSearchResultResult.Error.Message);

            return Result.Failure<SearchResult>(FlightSearchErrors.GetResultFailed);
        }

        var searchResult = getSearchResultResult.Value;
        if (searchResult.Failed)
        {
            _logger.LogInformation(
                "Результат поиска перелётов по ключу {SearchKey} от запроса {SearchRequestId} со статусом {SearchStatus} и ошибкой {ErrorCode}: {ErrorMessage}",
                request.SearchResultKey,
                request.Id,
                searchResult.Status.Code,
                searchResult.Error?.Code,
                searchResult.Error?.Message);
        }
        else
        {
            searchResult = searchResult.ApplyFiltersAndSorting(query.FilterOptions);
        }
        
        return searchResult;
    }
}
