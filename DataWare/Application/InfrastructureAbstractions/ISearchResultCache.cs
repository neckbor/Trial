using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;

namespace Application.InfrastructureAbstractions;

public interface ISearchResultCache
{
    Task SaveFlightsAsync(string searchKey, List<BaseFlight> flights);
    Task<Result<List<BaseFlight>>> GetFlightsAsync(string searchKey);
    Task SetProviderSearchStatusAsync(string searchKey, TicketingProvider provider, SearchStatus status);
    Task<Result<Dictionary<TicketingProvider, SearchStatus>>> GetProviderSearchStatusAsync(string searchKey);
}
