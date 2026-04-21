using Marketplace.Web.Models.Catalog;

namespace Marketplace.Web.Services.Catalog;

public interface ICatalogFilterEnricher
{
    Task<CatalogFilterVm> EnrichAsync(
        string culture,
        CatalogFilterVm filter,
        CancellationToken cancellationToken);
}