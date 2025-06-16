using Application.Booking;
using Application.Booking.DTOs;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
