using AutoMapper;
using Domain.Entities.Dictionaries;
using Domain.Repositories;

namespace DataAccess.Repositories;

internal class AirportRepository : EfRepository<Airport, long>, IAirportRepository
{
    public AirportRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }
}
