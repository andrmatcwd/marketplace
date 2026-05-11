namespace Marketplace.Modules.Listings.Domain.Entities;

public class ListingRental
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public string? Price { get; set; }
    public string? Rooms { get; set; }
    public string? Area { get; set; }
    public string? Floor { get; set; }

    public List<string> Features { get; set; } = new();

    public ICollection<ListingRentalRoom> RoomOptions { get; set; } = new List<ListingRentalRoom>();
}
