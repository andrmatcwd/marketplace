namespace Marketplace.Modules.Listings.Application.Rentals.Dtos;

public class RentalAdminDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string? Price { get; set; }
    public string? Rooms { get; set; }
    public string? Area { get; set; }
    public string? Floor { get; set; }
    public IReadOnlyList<string> Features { get; set; } = [];
    public IReadOnlyList<RentalRoomAdminDto> RoomOptions { get; set; } = [];
}
