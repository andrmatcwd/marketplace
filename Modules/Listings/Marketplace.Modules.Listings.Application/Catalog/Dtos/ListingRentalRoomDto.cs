namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingRentalRoomDto(
    string Title,
    string? Description,
    string? Price,
    string? Area,
    string? Guests,
    string? Beds,
    IReadOnlyList<string> ImageUrls,
    IReadOnlyList<string> Amenities);
