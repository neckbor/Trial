using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace DataAccess.Repositories;

internal class BookingRepository : EfRepository<Booking, Guid>, IBookingRepository
{
    public BookingRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }
}
