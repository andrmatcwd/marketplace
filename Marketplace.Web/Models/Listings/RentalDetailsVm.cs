namespace Marketplace.Web.Models.Listings;

public sealed class RentalDetailsVm
{
    public string? Price { get; set; }
    public string? Rooms { get; set; }
    public string? Area { get; set; }
    public string? Floor { get; set; }

    public IReadOnlyCollection<string> Features { get; set; } = [];
    public IReadOnlyCollection<RentalRoomVm> RoomOptions { get; set; } = [];
}