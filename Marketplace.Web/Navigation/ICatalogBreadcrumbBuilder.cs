using System;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Navigation;

public interface ICatalogBreadcrumbBuilder
{
    IReadOnlyList<BreadcrumbItemVm> Build(
        string? culture,
        params BreadcrumbSegment[] segments);
}
