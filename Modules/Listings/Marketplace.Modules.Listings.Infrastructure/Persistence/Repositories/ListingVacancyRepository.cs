using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingVacancyRepository : BaseRepository<ListingVacancy, int>, IListingVacancyRepository
{
    public ListingVacancyRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<ListingVacancy>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(v => v.ListingId == listingId)
            .OrderByDescending(v => v.Id)
            .ToListAsync(cancellationToken);
    }
}
