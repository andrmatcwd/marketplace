using System;

namespace Marketplace.Web.Models.Listings;

public class ListingCardVm
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string ShortDescription { get; init; } = default!;
    public string Slug { get; init; } = default!;

    public string SubCategoryName { get; init; } = default!;
    public string SubCategorySlug { get; init; } = default!;

    public double Rating { get; init; }
    public int ReviewsCount { get; init; }

    public string ImageUrl { get; set; } = string.Empty;

    public string Url { get; init; } = default!;
}
