using Application.Booking.DTOs;
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

    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(
        ILogger<BookingService> logger,
        IFlightSearchService flightSearchService,
        IFlightAggregator flightAggregator,
        IEnumerable<ITicketingProvider> ticketingProviders,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _flightSearchService = flightSearchService;
        _flightAggregator = flightAggregator;
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
            new List<Passenger>(),
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
                booking.ExternalBookkingId,
                booking.TicketingProvider.Code);

            return Result.Failure<Guid>(ApplicationErrors.General.Unexpected);
        }        
    }
}
