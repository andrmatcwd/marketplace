using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Mappings;

public sealed class CatalogVmMapper : ICatalogVmMapper
{
    public ListingCardVm MapListingCard(ListingCardDto dto, string culture)
    {
        return new ListingCardVm
        {
            Id = dto.Id,
            Title = dto.Title,
            Url = $"/{culture}/{dto.CitySlug}/{dto.CategorySlug}/{dto.SubCategorySlug}/{dto.Slug}/{dto.Id}",
            ImageUrl = dto.PrimaryImageUrl,
            ImageAlt = dto.PrimaryImageAlt ?? dto.Title,
            ShortDescription = dto.ShortDescription,
            CategoryName = dto.CategoryName,
            CategoryUrl = $"/{culture}/{dto.CitySlug}/{dto.CategorySlug}",
            SubCategoryName = dto.SubCategoryName,
            SubCategoryUrl = $"/{culture}/{dto.CitySlug}/{dto.CategorySlug}/{dto.SubCategorySlug}",
            CityName = dto.CityName,
            CityUrl = $"/{culture}/{dto.CitySlug}",
            Rating = dto.Rating,
            ReviewsCount = dto.ReviewsCount
        };
    }

    public CityCardVm MapCityCard(CatalogCityDto dto, string culture)
    {
        return new CityCardVm
        {
            Name = dto.Name,
            Url = $"/{culture}/{dto.Slug}",
            Description = dto.Description,
            ListingsCount = dto.ListingsCount
        };
    }

    public CategoryCardVm MapCategoryCard(CatalogCategoryDto dto, string culture, string? citySlug = null)
    {
        return new CategoryCardVm
        {
            Name = dto.Name,
            Url = string.IsNullOrWhiteSpace(citySlug)
                ? $"/{culture}/catalog?category={Uri.EscapeDataString(dto.Slug)}"
                : $"/{culture}/{citySlug}/{dto.Slug}",
            Description = dto.Description,
            ListingsCount = dto.ListingsCount
        };
    }

    public SubCategoryCardVm MapSubCategoryCard(CatalogSubCategoryDto dto, string culture, string citySlug)
    {
        return new SubCategoryCardVm
        {
            Name = dto.Name,
            Url = $"/{culture}/{citySlug}/{dto.CategorySlug}/{dto.Slug}",
            Description = dto.Description,
            ListingsCount = dto.ListingsCount
        };
    }

    public FilterOptionVm MapFilterOption(string value, string text)
    {
        return new FilterOptionVm
        {
            Value = value,
            Text = text
        };
    }
}
