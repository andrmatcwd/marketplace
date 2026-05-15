using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;

namespace Marketplace.Web.Mappings;

public sealed class CatalogVmMapper : ICatalogVmMapper
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogVmMapper(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public ListingCardVm MapListingCard(ListingCardDto dto, string culture)
    {
        return new ListingCardVm
        {
            Id = dto.Id,
            Title = dto.Title,
            Url = _urlBuilder.BuildListingUrl(culture, dto.CitySlug, dto.CategorySlug, dto.SubCategorySlug, dto.Slug, dto.Id),
            ImageUrl = dto.PrimaryImageUrl,
            ImageAlt = dto.PrimaryImageAlt ?? dto.Title,
            ShortDescription = dto.ShortDescription,
            CategoryName = dto.CategoryName,
            CategoryUrl = _urlBuilder.BuildCategoryUrl(culture, dto.CitySlug, dto.CategorySlug),
            SubCategoryName = dto.SubCategoryName,
            SubCategoryUrl = _urlBuilder.BuildSubCategoryUrl(culture, dto.CitySlug, dto.CategorySlug, dto.SubCategorySlug),
            CityName = dto.CityName,
            CityUrl = _urlBuilder.BuildCityUrl(culture, dto.CitySlug),
            Rating = dto.Rating,
            ReviewsCount = dto.ReviewsCount,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }

    public CityCardVm MapCityCard(CatalogCityDto dto, string culture)
    {
        return new CityCardVm
        {
            Name = dto.Name,
            Url = _urlBuilder.BuildCityUrl(culture, dto.Slug),
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
                ? $"{_urlBuilder.BuildCatalogUrl(culture)}?category={Uri.EscapeDataString(dto.Slug)}"
                : _urlBuilder.BuildCategoryUrl(culture, citySlug, dto.Slug),
            Description = dto.Description,
            ListingsCount = dto.ListingsCount
        };
    }

    public SubCategoryCardVm MapSubCategoryCard(CatalogSubCategoryDto dto, string culture, string citySlug)
    {
        return new SubCategoryCardVm
        {
            Name = dto.Name,
            Url = _urlBuilder.BuildSubCategoryUrl(culture, citySlug, dto.CategorySlug, dto.Slug),
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
