using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Services;

public interface ICatalogService
{
    public Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(
        string culture,
        CancellationToken cancellationToken);

    public Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string city,
        BaseFilter filter,
        CancellationToken cancellationToken);

    public Task<CategoryPageVm?> GetCategoryPageAsync(
        string culture,
        string city,
        string categorySlug,
        BaseFilter filter,
        CancellationToken cancellationToken);

    public Task<SubCategoryPageVm?> GetSubCategoryPageAsync(
        string culture,
        string city,
        string categorySlug,
        string subCategorySlug,
        BaseFilter filter,
        CancellationToken cancellationToken);
}
