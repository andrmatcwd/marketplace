using System.ComponentModel.DataAnnotations;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class Review : AuditedEntity
{
    public int Id { get; set; }

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public int? ReviewerId { get; set; }
    public Reviewer? Reviewer { get; set; }

    [MaxLength(120)]
    public string? AuthorName { get; set; }

    [MaxLength(2000)]
    public string? Text { get; set; }

    public double Rating { get; set; }
}
