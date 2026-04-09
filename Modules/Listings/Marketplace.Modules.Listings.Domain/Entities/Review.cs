namespace Marketplace.Modules.Listings.Domain.Entities;

public class Review : AuditedEntity
{
    public int Id { get; set; }

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public int ReviewerId { get; set; }
    public Reviewer Reviewer { get; set; } = null!;

    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
