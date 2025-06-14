using Application.Dictionaries.Airports;
using Application.Errors;
using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Primitives;
using Domain.Repositories;
using Domain.Shared;

namespace Application.FlightSearch;

internal class FlightSearchService : IFlightSearchService
{
    private readonly ISearchRequestRepository _searchRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IAirportService _airportService;

    public FlightSearchService(ISearchRequestRepository searchRequestRepository, IUnitOfWork unitOfWork, IAirportService airportService)
    {
        _searchRequestRepository = searchRequestRepository;
        _airportService = airportService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> CreateSearchRequestAsync(StartSearchCommand command)
    {
        var getFromAirportResult = await _airportService.GetByIATACodeAsync(command.FromAirportIATACode);
        if (getFromAirportResult.IsFailure) 
        {
            // log warning
            return Result.Failure<string>(getFromAirportResult.Error);
        }
        var fromAirport = getFromAirportResult.Value;

        var getToAirportResult = await _airportService.GetByIATACodeAsync(command.ToAirportIATACode);
        if (getToAirportResult.IsFailure) 
        {
            // log warning
            return Result.Failure<string>(getToAirportResult.Error);
        }
        var toAirport = getToAirportResult.Value;

        var createSearchRequestResult = SearchRequest.Create(command.ClientId, fromAirport, toAirport, command.DepartureDateUtc, command.PassengerCount);
        if (createSearchRequestResult.IsFailure) 
        {
            // log warning
            return Result.Failure<string>(createSearchRequestResult.Error);
        }
        var searchRequest = createSearchRequestResult.Value;

        try
        {
            await _searchRequestRepository.InsertAsync(searchRequest);
            await _unitOfWork.SaveChangesAsync();

            return searchRequest.SearchResultKey;
        }
        catch (Exception ex) 
        {
            // log error
            return Result.Failure<string>(ApplicationErrors.General.Unexpected);
        }
    }

}
