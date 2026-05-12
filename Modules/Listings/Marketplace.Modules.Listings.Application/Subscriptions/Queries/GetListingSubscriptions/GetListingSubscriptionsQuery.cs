using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetListingSubscriptions;

public sealed record GetListingSubscriptionsQuery(int ListingId) : IRequest<IReadOnlyList<ListingSubscriptionDto>>;
