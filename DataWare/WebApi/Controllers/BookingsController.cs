﻿using Application.Booking;
using Application.Booking.DTOs;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly LinkGenerator _linkGenerator;

        public BookingsController(IBookingService bookingService, LinkGenerator linkGenerator)
        {
            _bookingService = bookingService;
            _linkGenerator = linkGenerator;
        }

        [SwaggerOperation(
            Summary = "Забронировать перелёт",
            Description = "Для бронирования перелёта необходимо знать свой id запроса на поиск (он выдаётся после запроса POST /api/Search) " +
            "и id перелёта (поле FlightId из резльутатов поиска GET /api/Search/results)")]
        [HttpPost]
        public async Task<IResult> CreateAsync([FromBody] CreateBookingCommand command)
        {
            var result = await _bookingService.CreateBookingAsync(command);
            if (result.IsFailure)
            {
                return result.ToProblem();
            }

            var bookingId = result.Value;

            var location = _linkGenerator.GetPathByAction(
                HttpContext,
                action: nameof(GetAsync),
                controller: "FlightSearch",
                values: new { Id = bookingId });

            return Results.Created(location, bookingId);
        }

        [SwaggerOperation(
            Summary = "Получить информацию по броyированию",
            Description = "Для получения информации необходимо знать свой id бронирования (он выдаётся в результате работы POST /api/Bookings)")]
        [ActionName(nameof(GetAsync))]
        [HttpGet("{bookingId:guid}")]
        public async Task<IResult> GetAsync(Guid bookingId)
        {
            var result = await _bookingService.GetByIdAsync(bookingId);

            return result.IsSuccess ? Results.Ok(result.Value)
                                    : result.ToProblem();
        }
    }
}
