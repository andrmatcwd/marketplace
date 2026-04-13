using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.SubCategory;

namespace Marketplace.Web.Mappings;

public interface ICatalogVmMapper
{
    CategoryCardVm MapCategory(
        CategoryDto dto,
        string culture,
        string? citySlug = null);

    CityCardVm MapCity(
        CityDto dto,
        string culture);

    SubCategoryCardVm MapSubCategory(
        SubCategoryDto dto,
        string culture,
        string citySlug,
        string categorySlug);

    ListingCardVm MapListing(
        ListingDto dto,
        string culture,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null);
}
