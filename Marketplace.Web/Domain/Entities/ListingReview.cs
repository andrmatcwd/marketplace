namespace Marketplace.Web.Domain.Entities;

public sealed class ListingReview
{
    public Guid Id { get; set; }

    public Guid ListingId { get; set; }
    public Listing? Listing { get; set; }

    public string AuthorName { get; set; } = string.Empty;
    public string? Text { get; set; }

    public double Rating { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}