using AutoMapper;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.SubCategories.Mappings;

public sealed class SubCategoryMappingProfile : Profile
{
    public SubCategoryMappingProfile()
    {
        CreateMap<SubCategory, SubCategoryDto>();
    }
}
