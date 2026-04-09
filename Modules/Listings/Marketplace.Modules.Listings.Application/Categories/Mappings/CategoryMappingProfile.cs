using AutoMapper;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Categories.Mappings;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
