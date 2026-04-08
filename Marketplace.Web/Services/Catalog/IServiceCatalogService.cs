using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Services.Catalog;

public interface IServiceCatalogService
{
    Task<IReadOnlyList<ServiceCategoryViewModel>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<PagedResult<ServiceItemViewModel>> GetServicesAsync(
        ServicesFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceItemViewModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        ServiceItemViewModel model,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ServiceItemViewModel model,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}