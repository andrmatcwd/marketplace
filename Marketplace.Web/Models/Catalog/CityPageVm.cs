using System;
using Marketplace.Web.Areas.Admin.Models.Listings;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Models.Catalog;

public class CityPageVm
{
    // 📍 City
    public string CityName { get; init; } = default!;
    public string CitySlug { get; init; } = default!;

    // 🧠 SEO / Content
    public string H1 { get; init; } = default!;
    public string? IntroText { get; init; }

    public int TotalListingsCount { get; init; }
    public int TotalCategoriesCount { get; init; }

    // 📂 Categories (ГОЛОВНЕ на цій сторінці)
    public IReadOnlyCollection<CategoryItemVm> Categories { get; init; }
        = Array.Empty<CategoryItemVm>();

    // ⭐ (опціонально) популярні або нові лістинги
    public IReadOnlyCollection<ListingListItemVm> Listings { get; init; }
        = Array.Empty<ListingListItemVm>();

    // 🍞 Breadcrumbs
    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}