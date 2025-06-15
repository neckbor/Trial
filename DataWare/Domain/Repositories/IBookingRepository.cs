using Domain.Entities;
using Domain.Primitives;

namespace Domain.Repositories;

public interface IBookingRepository : IRepository<Booking, Guid>
{
}
