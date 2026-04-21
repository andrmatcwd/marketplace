namespace Marketplace.Web.Navigation;

public interface ICatalogUrlBuilder
{
    string BuildHomeUrl(string culture);

    string BuildCatalogUrl(string culture, int page = 1);
    string BuildCityUrl(string culture, string citySlug, int page = 1);
    string BuildCategoryUrl(string culture, string citySlug, string categorySlug, int page = 1);
    string BuildSubCategoryUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, int page = 1);

    string BuildListingUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, string listingSlug, Guid id);
}