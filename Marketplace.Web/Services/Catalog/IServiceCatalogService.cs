using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Services.Catalog;

public interface IServiceCatalogService
{
    Task<IReadOnlyList<ServiceCategoryViewModel>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceItemViewModel>> GetServicesAsync(
        ServicesFilterRequest request,
        CancellationToken cancellationToken = default);
}