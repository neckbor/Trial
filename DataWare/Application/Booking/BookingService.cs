using Application.Booking.DTOs;
using Application.Dictionaries.Countries;
using Application.Errors;
using Application.FlightAggregation;
using Application.FlightSearch;
using Application.InfrastructureAbstractions;
using Domain.Entities;
using Domain.Primitives;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Booking;

internal class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IFlightSearchService _flightSearchService;
    private readonly IFlightAggregator _flightAggregator;
    private readonly IEnumerable<ITicketingProvider> _ticketingProviders;
    private readonly ICountryService _countryService; 

    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(
        ILogger<BookingService> logger,
        IFlightSearchService flightSearchService,
        IFlightAggregator flightAggregator,
        ICountryService countryService,
        IEnumerable<ITicketingProvider> ticketingProviders,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _flightSearchService = flightSearchService;
        _flightAggregator = flightAggregator;
        _countryService = countryService;
        _ticketingProviders = ticketingProviders;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> CreateBookingAsync(CreateBookingCommand command)
    {
        var getSearchRequestResult = await _flightSearchService.GetSearchRequestByIdAsync(command.SearchRequestId);

        if (getSearchRequestResult.IsFailure)
        {
            _logger.LogWarning("Ошибка при получении запроса поиска {ErrorCode}: {ErrorMessage}",
                getSearchRequestResult.Error.Code,
                getSearchRequestResult.Error.Message);

            return Result.Failure<Guid>(getSearchRequestResult.Error);
        }

        var searchRequest = getSearchRequestResult.Value;

        if (searchRequest.PassengerCount != command.Passengers.Count)
        {
            _logger.LogWarning(
                "Количество пассажиров в запросе на поиск {SearchRequestId} ({ExpectedPassengerCount}) и переданное при создании бронирования ({ActualPassengerCount}) не совпадает",
                searchRequest.Id,
                searchRequest.PassengerCount,
                command.Passengers.Count);

            return Result.Failure<Guid>(BookingErrors.PassengerCountChanged);
        }

        var buildPassengersResult = await BuildPassengers(command.Passengers);
        if (buildPassengersResult.IsFailure)
        {
            _logger.LogWarning(
                "Ошибка при создании пассажиров {ErrorCode}: {ErrorMessage}",
                buildPassengersResult.Error.Code,
                buildPassengersResult.Error.Message);

            return Result.Failure<Guid>(buildPassengersResult.Error);
        }

        var passengers = buildPassengersResult.Value;

        var getFlightResult = await _flightAggregator.GetFlightByIdAsync(searchRequest, command.FlightId);
        if (getFlightResult.IsFailure) 
        {
            _logger.LogWarning(
                "Ошибка при получении перелёта {FlightId} {ErrorCode}: {ErrorMessage}",
                command.FlightId,
                getFlightResult.Error.Code,
                getFlightResult.Error.Message);

            return Result.Failure<Guid>(getFlightResult.Error);
        }

        var flight = getFlightResult.Value;

        var provider = _ticketingProviders.FirstOrDefault(p => p.Provider == flight.TicketingProvider);
        if (provider is null)
        {
            _logger.LogWarning("Не найдена имплементация провайдера {Provider}", flight.TicketingProvider.Code);

            return Result.Failure<Guid>(ApplicationErrors.General.PrividerUnavailable(flight.TicketingProvider));
        }

        var bookingResult = await provider.BookAsync(flight.FlightId, new List<Passenger>());
        if (bookingResult.IsFailure)
        {
            _logger.LogWarning(
                "Ошибка при бронировании перелёта {FlightId} у провайдера {Provider} {ErrorCode}: {ErrorMessage}",
                flight.FlightId,
                provider.Provider.Code,
                bookingResult.Error.Code,
                bookingResult.Error.Message);

            return Result.Failure<Guid>(BookingErrors.Failed);
        }

        var baseBooking = bookingResult.Value;

        var createBookingResult = Domain.Entities.Booking.Create(
            baseBooking.Provider,
            baseBooking.BookingId,
            flight,
            passengers,
            "");

        if (createBookingResult.IsFailure) 
        {
            _logger.LogWarning(
                "При создании бронирования {ExternalBookingId} провайдера {Provider} произошла ошибка {ErrorCode}: {ErrorMessage}",
                baseBooking.BookingId,
                baseBooking.Provider.Code,
                createBookingResult.Error.Code,
                createBookingResult.Error.Message);

            return Result.Failure<Guid>(BookingErrors.Failed);
        }

        var booking = createBookingResult.Value;

        try
        {
            await _bookingRepository.InsertAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Создано бронирование {BookingId}", booking.Id);

            return booking.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "При сохранении бронирования {ExternalBookingId} провайдера {Provider} вылетело исключение",
                booking.ExternalBookingId,
                booking.TicketingProvider.Code);

            return Result.Failure<Guid>(ApplicationErrors.General.Unexpected);
        }        
    }

    private async Task<Result<List<Passenger>>> BuildPassengers(List<PassengerInfo> passengersDto)
    {
        var passengers = new List<Passenger>();
        foreach(var p in passengersDto)
        {
            var getCountryResult = await _countryService.GetByCodeAsync(p.CountryCitizenshipCode);
            if (getCountryResult.IsFailure)
            {
                _logger.LogWarning(
                    "Ошибка при получении страны {ErrorCode}: {ErrorMessage}",
                    getCountryResult.Error.Code,
                    getCountryResult.Error.Message);

                return Result.Failure<List<Passenger>>(getCountryResult.Error);
            }

            var country = getCountryResult.Value;

            var createPassengerResult = Passenger.Create(p.FisrtName, p.LastName, p.MiddleName, p.DateOfBirth, p.Gender, p.PassportNumber, country);
            if (createPassengerResult.IsFailure)
            {
                _logger.LogWarning(
                    "Ошибка при создании пассажира {ErrorCode}: {ErrorMessage}",
                    createPassengerResult.Error.Code,
                    createPassengerResult.Error.Message);

                return Result.Failure<List<Passenger>>(createPassengerResult.Error);
            }

            passengers.Add(createPassengerResult.Value);
        }

        return passengers;
    }
}
