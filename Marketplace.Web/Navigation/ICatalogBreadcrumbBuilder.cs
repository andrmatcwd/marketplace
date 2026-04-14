using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Navigation;

public interface ICatalogBreadcrumbBuilder
{
    IReadOnlyCollection<BreadcrumbItemVm> BuildCatalog(string culture);
    IReadOnlyCollection<BreadcrumbItemVm> BuildCity(string culture, string cityName, string citySlug);
    IReadOnlyCollection<BreadcrumbItemVm> BuildCategory(string culture, string categoryName, string categorySlug, string cityName, string citySlug);
    IReadOnlyCollection<BreadcrumbItemVm> BuildSubCategory(
        string culture,
        string categoryName,
        string categorySlug,
        string subCategoryName,
        string subCategorySlug,
        string cityName,
        string citySlug);

    IReadOnlyCollection<BreadcrumbItemVm> BuildListing(
        string culture,
        string listingTitle,
        string cityName,
        string citySlug,
        string categoryName,
        string categorySlug,
        string subCategoryName,
        string subCategorySlug);
}