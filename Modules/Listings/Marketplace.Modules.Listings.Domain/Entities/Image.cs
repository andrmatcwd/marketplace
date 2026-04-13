using System.ComponentModel.DataAnnotations;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class Image
{
    public int Id { get; set; }

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }
}
