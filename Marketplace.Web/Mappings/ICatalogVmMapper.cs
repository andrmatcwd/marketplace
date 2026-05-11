using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Mappings;

public interface ICatalogVmMapper
{
    ListingCardVm MapListingCard(ListingCardDto dto, string culture);
    CityCardVm MapCityCard(CatalogCityDto dto, string culture);
    CategoryCardVm MapCategoryCard(CatalogCategoryDto dto, string culture, string? citySlug = null);
    SubCategoryCardVm MapSubCategoryCard(CatalogSubCategoryDto dto, string culture, string citySlug);
    FilterOptionVm MapFilterOption(string value, string text);
}
