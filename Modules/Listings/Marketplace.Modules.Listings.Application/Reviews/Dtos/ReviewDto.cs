namespace Marketplace.Modules.Listings.Application.Reviews.Dtos;

public class ReviewDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public int ReviewerId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
