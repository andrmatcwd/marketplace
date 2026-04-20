namespace Marketplace.Web.Navigation;

public interface ICatalogUrlBuilder
{
    string BuildHomeUrl(string culture);
    string BuildCatalogUrl(string culture);
    string BuildCityUrl(string culture, string citySlug);
    string BuildCategoryUrl(string culture, string citySlug, string categorySlug);
    string BuildSubCategoryUrl(string culture, string citySlug, string categorySlug, string subCategorySlug);
    string BuildListingUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, string listingSlug, Guid id);
}