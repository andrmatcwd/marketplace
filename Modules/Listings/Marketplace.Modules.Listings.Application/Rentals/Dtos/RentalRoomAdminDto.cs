namespace Marketplace.Modules.Listings.Application.Rentals.Dtos;

public class RentalRoomAdminDto
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Area { get; set; }
    public string? Guests { get; set; }
    public string? Beds { get; set; }
    public IReadOnlyList<string> Amenities { get; set; } = [];
    public IReadOnlyList<string> ImageUrls { get; set; } = [];
}
