using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Mappings;

public sealed class CatalogVmMapper : ICatalogVmMapper
{
    public ListingCardVm MapListingCard(Listing entity, string culture)
    {
        var citySlug = entity.City?.Slug ?? string.Empty;
        var categorySlug = entity.Category?.Slug ?? string.Empty;
        var subCategorySlug = entity.SubCategory?.Slug ?? string.Empty;

        return new ListingCardVm
        {
            Id = entity.Id,
            Title = entity.Title,
            Url = $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{entity.Slug}-{0}",
            ImageUrl = entity.Images
                .OrderBy(x => x.SortOrder)
                .FirstOrDefault(x => x.IsPrimary)?.Url
                ?? entity.Images.OrderBy(x => x.SortOrder).FirstOrDefault()?.Url,
            ImageAlt = entity.Images
                .OrderBy(x => x.SortOrder)
                .FirstOrDefault(x => x.IsPrimary)?.Alt
                ?? entity.Title,
            ShortDescription = entity.ShortDescription,
            CategoryName = entity.Category?.Name,
            CategoryUrl = entity.Category is null || entity.City is null
                ? null
                : $"/{culture}/{entity.City.Slug}/{entity.Category.Slug}",
            SubCategoryName = entity.SubCategory?.Name,
            SubCategoryUrl = entity.SubCategory is null || entity.Category is null || entity.City is null
                ? null
                : $"/{culture}/{entity.City.Slug}/{entity.Category.Slug}/{entity.SubCategory.Slug}",
            CityName = entity.City?.Name,
            CityUrl = entity.City is null ? null : $"/{culture}/{entity.City.Slug}",
            Rating = entity.Rating,
            ReviewsCount = entity.ReviewsCount
        };
    }

    public CityCardVm MapCityCard(City entity, int listingsCount, string culture)
    {
        return new CityCardVm
        {
            Name = entity.Name,
            Url = $"/{culture}/{entity.Slug}",
            Description = entity.Description,
            ListingsCount = listingsCount
        };
    }

    public CategoryCardVm MapCategoryCard(Category entity, int listingsCount, string culture, string? citySlug = null)
    {
        return new CategoryCardVm
        {
            Name = entity.Name,
            Url = string.IsNullOrWhiteSpace(citySlug)
                ? $"/{culture}/catalog?category={Uri.EscapeDataString(entity.Slug)}"
                : $"/{culture}/{citySlug}/{entity.Slug}",
            Description = entity.Description,
            ListingsCount = listingsCount
        };
    }

    public SubCategoryCardVm MapSubCategoryCard(SubCategory entity, int listingsCount, string culture, string citySlug)
    {
        return new SubCategoryCardVm
        {
            Name = entity.Name,
            Url = entity.Category is null
                ? "#"
                : $"/{culture}/{citySlug}/{entity.Category.Slug}/{entity.Slug}",
            Description = entity.Description,
            ListingsCount = listingsCount
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