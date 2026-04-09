namespace Marketplace.Modules.Listings.Application.Reviews.Filters;

public sealed class ReviewFilter
{
    public int? ListingId { get; set; }
    public int? ReviewerId { get; set; }
    public int? Rating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
