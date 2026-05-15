namespace Marketplace.Modules.Listings.Application.Reviews.Dtos;

public class ReviewDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public string? Text { get; set; }
    public double Rating { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
