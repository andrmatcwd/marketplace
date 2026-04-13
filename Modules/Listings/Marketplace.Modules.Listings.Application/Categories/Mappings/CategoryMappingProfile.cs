using AutoMapper;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Categories.Mappings;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City.Name))
            .ForMember(d => d.CitySlug, opt => opt.MapFrom(s => s.City.Slug))
            .ForMember(d => d.ListingsCount, opt => opt.MapFrom(s => s.Listings.Count))
            .ForMember(d => d.SubCategoriesCount, opt => opt.MapFrom(s => s.SubCategories.Count));
    }
}
