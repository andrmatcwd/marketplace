using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Services.Catalog;

public interface ICatalogPaginationBuilder
{
    PaginationVm Build(
        CatalogFilterVm filter,
        int totalItems,
        Func<int, string> buildPath);
}