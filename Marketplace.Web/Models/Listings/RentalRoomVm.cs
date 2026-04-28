namespace Marketplace.Web.Models.Listings;

public sealed class RentalRoomVm
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public IReadOnlyCollection<string> ImageUrls { get; set; } = [];

    public string? Price { get; set; }
    public string? Area { get; set; }
    public string? Guests { get; set; }
    public string? Beds { get; set; }

    public IReadOnlyCollection<string> Amenities { get; set; } = [];
}