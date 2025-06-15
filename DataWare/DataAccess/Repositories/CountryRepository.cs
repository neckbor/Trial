using AutoMapper;
using Domain.Entities.Dictionaries;
using Domain.Repositories;

namespace DataAccess.Repositories;

internal class CountryRepository : EfRepository<Country, int>, ICountryRepository
{
    public CountryRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }
}
