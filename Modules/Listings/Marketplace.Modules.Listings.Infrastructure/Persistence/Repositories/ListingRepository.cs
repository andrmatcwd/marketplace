using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingRepository
    : BaseRepository<Listing, int>, IListingRepository
{
    public ListingRepository(ListingsDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<(IReadOnlyCollection<Listing> Items, int TotalCount)> GetByFilterAsync(
        ListingFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(x => x.Title.Contains(filter.Search));
        if (filter.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
        if (filter.SubCategoryId.HasValue)
            query = query.Where(x => x.SubCategoryId == filter.SubCategoryId.Value);
        if (filter.CityId.HasValue)
            query = query.Where(x => x.CityId == filter.CityId.Value);
        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);
        if (!string.IsNullOrWhiteSpace(filter.SellerId))
            query = query.Where(x => x.SellerId == filter.SellerId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
