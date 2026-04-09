namespace Marketplace.Modules.Listings.Domain.Entities;

public class Image
{
    public int Id { get; set; }

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public string Url { get; set; } = string.Empty;
    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }
}
