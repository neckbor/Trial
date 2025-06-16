using AutoMapper;

namespace Application.Booking.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Booking, BookingDto>();
    }
}
