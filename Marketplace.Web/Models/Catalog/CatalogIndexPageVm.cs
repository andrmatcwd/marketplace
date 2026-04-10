using System;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Models.Catalog;

public class CatalogIndexPageVm
{
    public string H1 { get; init; } = default!;
    public string? IntroText { get; init; }

    public IReadOnlyCollection<CityItemVm> Cities { get; init; }
        = Array.Empty<CityItemVm>();

    public IReadOnlyCollection<CategoryItemVm> PopularCategories { get; init; }
        = Array.Empty<CategoryItemVm>();

    // 🔍 Filters
    public BaseFilter Filter { get; init; } = new();

    // 📄 Pagination
    public PaginationVm Pagination { get; init; } = new();

    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}
