using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;

public sealed record CreateListingCommand(
    string Title,
    string Description,
    string Name,
    string SellerId,
    string AddressLine,
    double? Latitude,
    double? Longitude,
    SubscriptionType SubscriptionType,
    ListingStatus Status,
    int CategoryId,
    int SubCategoryId,
    int CityId
) : IRequest<Unit>;