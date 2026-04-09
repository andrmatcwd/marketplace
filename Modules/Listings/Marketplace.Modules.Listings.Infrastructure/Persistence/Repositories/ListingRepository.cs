using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingRepository
    : BaseRepository<Listing, Guid>, IListingRepository
{
    public ListingRepository(ListingsDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<(IReadOnlyCollection<Listing> Items, int TotalCount)> GetListingsAsync(
        ListingFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Location)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x => x.Title.Contains(filter.Search));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.Status == ListingStatus.Active);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
