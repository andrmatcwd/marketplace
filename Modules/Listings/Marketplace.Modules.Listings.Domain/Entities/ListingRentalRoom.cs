namespace Marketplace.Modules.Listings.Domain.Entities;

public class ListingRentalRoom
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public ListingRental Rental { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Area { get; set; }
    public string? Guests { get; set; }
    public string? Beds { get; set; }

    public List<string> Amenities { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
}
