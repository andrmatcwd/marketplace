using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class SubscriptionPlanRepository
    : BaseRepository<SubscriptionPlan, int>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(ListingsDbContext dbContext) : base(dbContext) { }

    public async Task<IReadOnlyList<SubscriptionPlan>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }
}
