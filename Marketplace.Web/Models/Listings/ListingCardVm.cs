using System;

namespace Marketplace.Web.Models.Listings;

public class ListingCardVm
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;

    public string SubcategoryName { get; init; } = default!;
    public string SubcategorySlug { get; init; } = default!;

    public decimal? Rating { get; init; }
    public int ReviewsCount { get; init; }

    public string Url { get; init; } = default!;
}
