using Marketplace.Web.Models.Catalog;

namespace Marketplace.Web.Services.Catalog;

public interface ICatalogService
{
    Task<CatalogGatewayPageVm> GetCatalogGatewayPageAsync(string culture, string? selectedCitySlug, CancellationToken cancellationToken);
    Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(string culture, CatalogFilterVm filter, CancellationToken cancellationToken);
    Task<CityPageVm?> GetCityPageAsync(string culture, string citySlug, CatalogFilterVm filter, CancellationToken cancellationToken);
    Task<CategoryPageVm?> GetCategoryPageAsync(string culture, string citySlug, string categorySlug, CatalogFilterVm filter, CancellationToken cancellationToken);
    Task<SubCategoryPageVm?> GetSubCategoryPageAsync(string culture, string citySlug, string categorySlug, string subCategorySlug, CatalogFilterVm filter, CancellationToken cancellationToken);
}