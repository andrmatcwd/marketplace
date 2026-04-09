using AutoMapper;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Listings.Mappings;

public sealed class ListingMappingProfile : Profile
{
    public ListingMappingProfile()
    {
        CreateMap<Listing, ListingDto>()
            .ForMember(d => d.CategoryName,
                opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.LocationName,
                opt => opt.MapFrom(s => s.Location.Name));
    }
}
