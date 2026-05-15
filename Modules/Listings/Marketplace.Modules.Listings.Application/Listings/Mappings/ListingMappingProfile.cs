using AutoMapper;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Listings.Mappings;

public sealed class ListingMappingProfile : Profile
{
    public ListingMappingProfile()
    {
        CreateMap<Listing, ListingDto>()
            .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(src => src.Reviews.Count))
            .ForMember(dest => dest.RatingAverage,
                opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category.Slug))
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Name : string.Empty))
            .ForMember(dest => dest.SubCategorySlug, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Slug : string.Empty))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.CitySlug, opt => opt.MapFrom(src => src.City.Slug))
            .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
    }
}
