using System;
using Marketplace.Web.Areas.Admin.Models.Listings;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.SubCategory;

namespace Marketplace.Web.Models.Catalog;

public class CategoryPageVm
{
    // 📍 Location
    public string CityName { get; init; } = default!;
    public string CitySlug { get; init; } = default!;

    // 📂 Category
    public string CategoryName { get; init; } = default!;
    public string CategorySlug { get; init; } = default!;

    // 🧠 UI / SEO content
    public string H1 { get; init; } = default!;
    public string? IntroText { get; init; }

    public int TotalListingsCount { get; init; }
    public int TotalSubCategoiesCount { get; init; }

    // 📊 Subcategories (дуже важливо для SEO)
    public IReadOnlyCollection<SubCategoryItemVm> SubCategories { get; init; }
        = Array.Empty<SubCategoryItemVm>();

    // 📦 Listings (якщо показуєш на цій сторінці)
    public IReadOnlyCollection<ListingListItemVm> Listings { get; init; }
        = Array.Empty<ListingListItemVm>();

    // 🍞 Breadcrumbs
    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}
