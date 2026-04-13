using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.SubCategory;
using Marketplace.Web.Navigation;

namespace Marketplace.Web.Mappings;


public sealed class CatalogVmMapper : ICatalogVmMapper
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogVmMapper(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public CategoryCardVm MapCategory(
        CategoryDto dto,
        string culture,
        string? citySlug = null)
    {
        return new CategoryCardVm
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            Icon = dto.Icon,
            ListingsCount = dto.ListingsCount,
            SubCategoryCount = dto.SubCategoriesCount,
            Url = citySlug is null
                ? _urlBuilder.Build(culture: culture, categorySlug: dto.Slug)
                : _urlBuilder.BuildCategory(culture, citySlug, dto.Slug)
        };
    }

    public CityCardVm MapCity(
        CityDto dto,
        string culture)
    {
        return new CityCardVm
        {
            Name = dto.Name,
            Slug = dto.Slug,
            RegionName = dto.RegionName,
            RegionSlug = dto.RegionSlug,
            ListingsCount = dto.ListingsCount,
            CategoriesCount = dto.CategoriesCount,
            Url = _urlBuilder.BuildCity(culture, dto.Slug)
        };
    }

    public SubCategoryCardVm MapSubCategory(
        SubCategoryDto dto,
        string culture,
        string citySlug,
        string categorySlug)
    {
        return new SubCategoryCardVm
        {
            Name = dto.Name,
            Slug = dto.Slug,
            ListingsCount = dto.ListingsCount,
            Url = _urlBuilder.BuildSubCategory(culture, citySlug, categorySlug, dto.Slug)
        };
    }

    public ListingCardVm MapListing(
        ListingDto dto,
        string culture,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null)
    {
        return new ListingCardVm
        {
            Id = dto.Id,
            Title = dto.Title,
            Slug = dto.Slug,
            ShortDescription = dto.Description,
            Url = _urlBuilder.Build(
                culture: culture,
                citySlug: citySlug,
                categorySlug: categorySlug,
                subCategorySlug: subCategorySlug,
                listingSlug: dto.Slug,
                listingId: dto.Id)
        };
    }
}
