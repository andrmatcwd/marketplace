using AutoMapper;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Cities.Mappings;

public sealed class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<City, CityDto>();
    }
}
