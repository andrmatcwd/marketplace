namespace Marketplace.Web.Navigation;

public sealed class CatalogUrlBuilder : ICatalogUrlBuilder
{
    public string BuildHomeUrl(string culture)
        => $"/{culture}";

    public string BuildCatalogUrl(string culture)
        => $"/{culture}/catalog";

    public string BuildCityUrl(string culture, string citySlug)
        => $"/{culture}/{citySlug}";

    public string BuildCategoryUrl(string culture, string citySlug, string categorySlug)
        => $"/{culture}/{citySlug}/{categorySlug}";

    public string BuildSubCategoryUrl(string culture, string citySlug, string categorySlug, string subCategorySlug)
        => $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}";

    public string BuildListingUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, string listingSlug, Guid id)
        => $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{listingSlug}-{id}";
}