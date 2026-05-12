using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.Subscriptions.Filters;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingSubscriptionRepository
    : BaseRepository<ListingSubscription, int>, IListingSubscriptionRepository
{
    public ListingSubscriptionRepository(ListingsDbContext dbContext) : base(dbContext) { }

    public async Task<(IReadOnlyCollection<ListingSubscription> Items, int TotalCount)> GetByFilterAsync(
        SubscriptionFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .AsNoTracking()
            .Include(x => x.Listing)
            .Include(x => x.Plan)
            .AsQueryable();

        if (filter.ListingId.HasValue)
            query = query.Where(x => x.ListingId == filter.ListingId.Value);
        if (filter.PlanId.HasValue)
            query = query.Where(x => x.PlanId == filter.PlanId.Value);
        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IReadOnlyList<ListingSubscription>> GetByListingIdAsync(
        int listingId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(x => x.Plan)
            .Where(x => x.ListingId == listingId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ListingSubscription>> GetExpiredActiveAsync(
        DateTime asOf,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x => x.Status == SubscriptionStatus.Active && x.ExpiresAt <= asOf)
            .ToListAsync(cancellationToken);
    }
}
