using AutoMapper;
using Marketplace.Modules.Listings.Application.Locations.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Locations.Mappings;

public sealed class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<Location, LocationDto>();
    }
}
