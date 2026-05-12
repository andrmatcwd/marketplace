using Marketplace.Modules.Listings.Application.Subscriptions.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IListingSubscriptionRepository : IBaseRepository<ListingSubscription, int>
{
    Task<(IReadOnlyCollection<ListingSubscription> Items, int TotalCount)> GetByFilterAsync(
        SubscriptionFilter filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ListingSubscription>> GetByListingIdAsync(
        int listingId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ListingSubscription>> GetExpiredActiveAsync(
        DateTime asOf,
        CancellationToken cancellationToken = default);
}
