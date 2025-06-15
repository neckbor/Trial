using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace DataAccess.Repositories;

internal class SearchRequestRepository : EfRepository<SearchRequest, Guid>, ISearchRequestRepository
{
    public SearchRequestRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }
}
