using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;

public sealed record EditListingCommand(
    int Id,
    string Title,
    string? Slug,
    string? ShortDescription,
    string? Description,
    string? Phone,
    string? Email,
    string? Website,
    string? Address,
    double? Latitude,
    double? Longitude,
    string? SellerId,
    SubscriptionType SubscriptionType,
    ListingStatus Status,
    int CategoryId,
    int SubCategoryId,
    int CityId
) : IRequest<Unit>;
