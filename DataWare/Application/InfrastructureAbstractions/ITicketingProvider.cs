using Application.FlightSearch.DTOs;
using Domain.Entities;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Application.InfrastructureAbstractions;

public interface ITicketingProvider
{
    TicketingProvider Provider { get; }
    Task<Result<List<BaseFlight>>> SearchAsync(SearchRequestDto request);
    Task<Result<BaseBooking>> BookAsync(string flightId, List<Passenger> passengers);
}
