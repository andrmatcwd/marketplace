using System;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Services;

public class CatalogService : ICatalogService
{
    public Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryPageVm?> GetCategoryPageAsync(string city, string categorySlug, BaseFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<CityPageVm?> GetCityPageAsync(string city, BaseFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SubCategoryPageVm?> GetSubCategoryPageAsync(string city, string categorySlug, string subCategorySlug, BaseFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
