using AutoMapper;
using Domain.Entities;
using Domain.Entities.Dictionaries;

namespace Application.FlightSearch.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Airport, AirportDto>();

        CreateMap<SearchRequest, SearchRequestDto>();
    }
}
