using System;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class CityRepository
    : BaseRepository<City, int>, ICityRepository
{
    public CityRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IReadOnlyCollection<City> Items, int TotalCount)> GetByFilterAsync(
        CityFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(x => x.Region)
            .Include(x => x.Categories)
            .Include(x => x.Listings)
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
