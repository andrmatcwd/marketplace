using System;

namespace Marketplace.Web.Models.Listings;

public class ListingReviewVm
{
    public string AuthorName { get; init; } = default!;
    public decimal Rating { get; init; }
    public string? Text { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
