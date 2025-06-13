using Domain.Models;

namespace Application.InfrastructureAbstractions;

public interface ISearchResultCache
{
    Task SaveAsync(Guid searchId, List<BaseFlight> flights);
    Task<List<BaseFlight>> GetAsync(Guid searchId);
}
