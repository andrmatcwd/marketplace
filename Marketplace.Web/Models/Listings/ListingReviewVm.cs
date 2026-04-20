namespace Marketplace.Web.Models.Listings;

public sealed class ListingReviewVm
{
    public string AuthorName { get; set; } = string.Empty;
    public string? Text { get; set; }

    public double Rating { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public bool HasText => !string.IsNullOrWhiteSpace(Text);
    public string RatingFormatted => Rating > 0 ? Rating.ToString("0.0") : string.Empty;
}