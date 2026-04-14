using Marketplace.Web.Models.Catalog;

namespace Marketplace.Web.Seo;

public sealed class SeoIndexingPolicy
{
    public bool ShouldNoIndex(CatalogFilterVm filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            return true;
        }

        if (filter.Page > 1)
        {
            return true;
        }

        return false;
    }

    public string GetRobots(CatalogFilterVm filter)
    {
        return ShouldNoIndex(filter)
            ? "noindex, follow"
            : "index, follow";
    }
}