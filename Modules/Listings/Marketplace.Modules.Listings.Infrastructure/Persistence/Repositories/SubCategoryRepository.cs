using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository
    : BaseRepository<SubCategory, int>, ISubCategoryRepository
{
    public SubCategoryRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IReadOnlyCollection<SubCategory> Items, int TotalCount)> GetByFilterAsync(
        SubCategoryFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(x => x.Category)
            .Include(x => x.Listings)
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
