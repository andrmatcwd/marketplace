using AutoMapper;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.SubCategories.Mappings;

public sealed class SubCategoryMappingProfile : Profile
{
    public SubCategoryMappingProfile()
    {
        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category.Slug))
            .ForMember(dest => dest.ListingsCount, opt => opt.MapFrom(src => src.Listings.Count));
    }
}
