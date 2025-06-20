﻿using Application.FlightSearch;
using Application.FlightSearch.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Contracts.FlightSearch;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IFlightSearchService _flightSearchService;
        private readonly LinkGenerator _linkGenerator;

        public SearchController(IFlightSearchService flightSearchService, LinkGenerator linkGenerator)
        {
            _flightSearchService = flightSearchService;
            _linkGenerator = linkGenerator;
        }

        [SwaggerOperation(
            Summary = "Создать запрос на поиск перелётов (Надётся билет на дату 2025-07-01 из LAX в CDG)",
            Description = "После создания запроса на поиск система пришлёт вам id вышего запроса. Его необходимо отправить в GET /api/Sarch/results для получения результатов")]
        [HttpPost]
        public async Task<IResult> PostSearchAsync(CreateFlightSearchRequest request)
        {
            var command = new StartSearchCommand(Guid.NewGuid().ToString(), request.DepartureDate, request.FromIATA, request.ToIATA, request.PassengerCount);

            var result = await _flightSearchService.CreateSearchRequestAsync(command);

            if (result.IsFailure)
            {
                return result.ToProblem();
            }

            Guid searchRequestId = result.Value;

            var location = _linkGenerator.GetPathByAction(
                HttpContext,
                action: nameof(GetSearchResultsAsync),
                controller: "Search",
                values: new { Id = searchRequestId });

            return Results.Created(location, searchRequestId);
        }

        [SwaggerOperation(
            Summary = "Получить результаты поиска",
            Description = "Для получения результатов поиска по вашему запросу, необходимо отправить id вашего запроса, который был сгенерирован после вызова POST /api/Search." +
            "Для бронирования перелёта, необходимо вызвать метод POST /api/Bookings, отправив в него id вашего запроса на поиск и id конркетного перелёта из представленных результатов " +
            "(поле FlightId)")]
        [ActionName(nameof(GetSearchResultsAsync))]
        [HttpGet("results/{requestId:guid}")]
        public async Task<IResult> GetSearchResultsAsync(Guid requestId, 
            [FromQuery] int maxTranfers,
            [FromQuery] decimal? maxPrice,  
            [FromQuery] List<string>? airlineCodes, 
            [FromQuery] FlightSearchResultSortOption? sortOption, 
            [FromQuery] bool? sortDescending)
        {
            var query = new GetSearchResultsQuery(requestId, new SearchResultFilterOptions
            {
                AirlineCodes = airlineCodes,
                MaxPrice = maxPrice,
                MaxTransfers = maxTranfers,
                SortOption = sortOption ?? FlightSearchResultSortOption.Price,
                SortDescending = sortDescending ?? false
            });

            var result = await _flightSearchService.GetSearchResultAsync(query);

            return result.IsSuccess ? Results.Ok(result.Value) 
                                    : result.ToProblem();
        }
    }
}
