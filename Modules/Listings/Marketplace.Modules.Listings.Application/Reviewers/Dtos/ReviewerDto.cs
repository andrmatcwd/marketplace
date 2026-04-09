namespace Marketplace.Modules.Listings.Application.Reviewers.Dtos;

public class ReviewerDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ReviewsCount { get; set; }
    public double AverageGivenRating { get; set; }
    public bool IsActive { get; set; }
}
