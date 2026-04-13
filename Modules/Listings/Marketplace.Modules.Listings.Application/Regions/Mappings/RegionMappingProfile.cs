using AutoMapper;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Regions.Mappings;

public sealed class RegionMappingProfile : Profile
{
    public RegionMappingProfile()
    {
        CreateMap<Region, RegionDto>()
            .ForMember(dest => dest.CitiesCount, opt => opt.MapFrom(src => src.Cities.Count));
    }
}
