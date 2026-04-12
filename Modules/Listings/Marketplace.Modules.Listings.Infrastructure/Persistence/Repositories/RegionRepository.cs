using System;
using Marketplace.Modules.Listings.Application.Regions.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class RegionRepository
    : BaseRepository<Region, int>, IRegionRepository
{
    public RegionRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IReadOnlyCollection<Region> Items, int TotalCount)> GetByFilterAsync(
        RegionFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
