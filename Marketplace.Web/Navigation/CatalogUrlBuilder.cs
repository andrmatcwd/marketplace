namespace Marketplace.Web.Navigation;

public sealed class CatalogUrlBuilder : ICatalogUrlBuilder
{
    public string BuildHomeUrl(string culture)
        => $"/{culture}";

    public string BuildCatalogUrl(string culture, int page = 1)

        => page <= 1

            ? $"/{culture}/catalog"

            : $"/{culture}/catalog/page-{page}";

    public string BuildCityUrl(string culture, string citySlug, int page = 1)

        => page <= 1

            ? $"/{culture}/{citySlug}"

            : $"/{culture}/{citySlug}/page-{page}";

    public string BuildCategoryUrl(string culture, string citySlug, string categorySlug, int page = 1)

        => page <= 1

            ? $"/{culture}/{citySlug}/{categorySlug}"

            : $"/{culture}/{citySlug}/{categorySlug}/page-{page}";

    public string BuildSubCategoryUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, int page = 1)

        => page <= 1

            ? $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}"

            : $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/page-{page}";

    public string BuildListingUrl(string culture, string citySlug, string categorySlug, string subCategorySlug, string listingSlug, int id)
        => $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{listingSlug}/{id}";

    public string BuildVacanciesUrl(string culture, int page = 1)
        => page <= 1
            ? $"/{culture}/vacancies"
            : $"/{culture}/vacancies/page-{page}";
}