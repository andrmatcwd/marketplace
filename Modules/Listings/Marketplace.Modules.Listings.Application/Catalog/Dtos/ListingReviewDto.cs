namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingReviewDto(
    string AuthorName,
    string? Text,
    double Rating,
    DateTime CreatedAtUtc);
