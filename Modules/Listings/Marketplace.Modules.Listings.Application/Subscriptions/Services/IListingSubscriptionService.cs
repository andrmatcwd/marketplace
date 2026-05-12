using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Filters;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Services;

public interface IListingSubscriptionService
{
    Task<PagedResult<ListingSubscriptionDto>> GetByFilterAsync(SubscriptionFilter filter, CancellationToken cancellationToken);
    Task<IReadOnlyList<ListingSubscriptionDto>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken);
    Task AssignAsync(int listingId, int planId, DateTime startsAt, string? assignedByUserId, string? notes, CancellationToken cancellationToken);
    Task CancelAsync(int subscriptionId, CancellationToken cancellationToken);
    Task ExpireOverdueAsync(CancellationToken cancellationToken);
}
