using AutoMapper;
using Domain.Entities.Dictionaries;
using Domain.Repositories;

namespace DataAccess.Repositories;

internal class AirlineRepository : EfRepository<Airline, int>, IAirlineRepository
{
    public AirlineRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }
}
