using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ImageRepository : BaseRepository<Image, int>, IImageRepository
{
    public ImageRepository(ListingsDbContext dbContext) : base(dbContext) { }

    public async Task<IReadOnlyList<Image>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(i => i.ListingId == listingId)
            .OrderBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .ToListAsync(cancellationToken);
    }
}
