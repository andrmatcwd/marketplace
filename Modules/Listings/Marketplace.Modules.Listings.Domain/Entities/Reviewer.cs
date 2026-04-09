namespace Marketplace.Modules.Listings.Domain.Entities;

public sealed class Reviewer : AuditedEntity
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int ReviewsCount { get; set; }
    public double AverageGivenRating { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
