using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Mappings;

public interface ICatalogVmMapper
{
    ListingCardVm MapListingCard(Listing entity, string culture);
    CityCardVm MapCityCard(City entity, int listingsCount, string culture);
    CategoryCardVm MapCategoryCard(Category entity, int listingsCount, string culture, string? citySlug = null);
    SubCategoryCardVm MapSubCategoryCard(SubCategory entity, int listingsCount, string culture, string citySlug);
    FilterOptionVm MapFilterOption(string value, string text);
}