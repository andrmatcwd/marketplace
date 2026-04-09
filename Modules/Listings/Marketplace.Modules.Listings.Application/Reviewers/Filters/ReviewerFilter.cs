namespace Marketplace.Modules.Listings.Application.Reviewers.Filters;

public sealed class ReviewerFilter
{
    public string? UserId { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
