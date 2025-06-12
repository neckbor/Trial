using Domain.Entities;
using Domain.Primitives;

namespace Domain.Repositories;

public interface ISearchRequestRepository : IRepository<SearchRequest, Guid>
{
}
