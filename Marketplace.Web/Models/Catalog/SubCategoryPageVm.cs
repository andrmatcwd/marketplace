using System;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Models.Catalog;

public class SubCategoryPageVm
{
    // Location
    public string CityName { get; init; } = default!;
    public string CitySlug { get; init; } = default!;

    // Category
    public string CategoryName { get; init; } = default!;
    public string CategorySlug { get; init; } = default!;

    // Subcategory
    public string SubcategoryName { get; init; } = default!;
    public string SubcategorySlug { get; init; } = default!;

    // SEO / Content
    public string H1 { get; init; } = default!;
    public string? IntroText { get; init; }

    // Stats
    public int TotalListingsCount { get; init; }

    // Filters
    public BaseFilter Filter { get; init; } = new();

    // Results
    public IReadOnlyCollection<ListingCardVm> Listings { get; init; }
        = Array.Empty<ListingCardVm>();

    // Pagination
    public PaginationVm Pagination { get; init; } = new();

    // Optional internal linking
    // public IReadOnlyCollection<SubcategoryLinkVm> RelatedSubcategories { get; init; }
    //     = Array.Empty<SubcategoryLinkVm>();

    // Breadcrumbs
    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}
