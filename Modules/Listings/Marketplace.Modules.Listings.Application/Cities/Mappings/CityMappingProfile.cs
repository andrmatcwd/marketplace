using AutoMapper;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Cities.Mappings;

public sealed class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<City, CityDto>()
            .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name))
            .ForMember(dest => dest.RegionSlug, opt => opt.MapFrom(src => src.Region.Slug))
            .ForMember(dest => dest.ListingsCount, opt => opt.MapFrom(src => src.Listings.Count))
            .ForMember(dest => dest.CategoriesCount, opt => opt.Ignore());
    }
}
