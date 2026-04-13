using System;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Navigation;

public sealed class CatalogBreadcrumbBuilder : ICatalogBreadcrumbBuilder
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogBreadcrumbBuilder(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public IReadOnlyList<BreadcrumbItemVm> Build(
        string? culture,
        params BreadcrumbSegment[] segments)
    {
        var validSegments = segments
            .Where(x => !string.IsNullOrWhiteSpace(x.Title))
            .ToList();

        if (validSegments.Count == 0)
        {
            return [];
        }

        var items = new List<BreadcrumbItemVm>(validSegments.Count);

        for (var i = 0; i < validSegments.Count; i++)
        {
            items.Add(new BreadcrumbItemVm
            {
                Title = validSegments[i].Title,
                Url = i == validSegments.Count - 1
                    ? null
                    : BuildBreadcrumbUrl(culture, validSegments, i),
                IsCurrent = i == validSegments.Count - 1
            });
        }

        return items;
    }

    private string BuildBreadcrumbUrl(
        string? culture,
        IReadOnlyList<BreadcrumbSegment> segments,
        int lastIndexInclusive)
    {
        string? citySlug = null;
        string? categorySlug = null;
        string? subCategorySlug = null;
        string? listingSlug = null;
        int? listingId = null;

        for (var i = 0; i <= lastIndexInclusive; i++)
        {
            switch (segments[i].Type)
            {
                case BreadcrumbSegmentType.City:
                    citySlug = segments[i].Slug;
                    break;
                case BreadcrumbSegmentType.Category:
                    categorySlug = segments[i].Slug;
                    break;
                case BreadcrumbSegmentType.SubCategory:
                    subCategorySlug = segments[i].Slug;
                    break;
                case BreadcrumbSegmentType.Listing:
                    listingSlug = segments[i].Slug;
                    listingId = segments[i].ListingId;
                    break;
            }
        }

        return _urlBuilder.Build(
            culture: culture,
            citySlug: citySlug,
            categorySlug: categorySlug,
            subCategorySlug: subCategorySlug,
            listingSlug: listingSlug,
            listingId: listingId);
    }
}

public sealed record BreadcrumbSegment(
    string Title,
    BreadcrumbSegmentType Type,
    string? Slug = null,
    int? ListingId = null);

public enum BreadcrumbSegmentType
{
    Root = 0,
    City = 1,
    Category = 2,
    SubCategory = 3,
    Listing = 4
}

public static class BreadcrumbSegments
{
    public static BreadcrumbSegment Root(string title)
        => new(title, BreadcrumbSegmentType.Root);

    public static BreadcrumbSegment City(string title, string slug)
        => new(title, BreadcrumbSegmentType.City, slug);

    public static BreadcrumbSegment Category(string title, string slug)
        => new(title, BreadcrumbSegmentType.Category, slug);

    public static BreadcrumbSegment SubCategory(string title, string slug)
        => new(title, BreadcrumbSegmentType.SubCategory, slug);

    public static BreadcrumbSegment Listing(string title, string slug, int listingId)
        => new(title, BreadcrumbSegmentType.Listing, slug, listingId);
}
