namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingRentalDto(
    string? Price,
    string? Rooms,
    string? Area,
    string? Floor,
    IReadOnlyList<string> Features,
    IReadOnlyList<ListingRentalRoomDto> RoomOptions);
