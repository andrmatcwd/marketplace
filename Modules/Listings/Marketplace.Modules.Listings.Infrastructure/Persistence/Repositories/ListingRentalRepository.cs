using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingRentalRepository : BaseRepository<ListingRental, int>, IListingRentalRepository
{
    public ListingRentalRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<ListingRental?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.RoomOptions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<ListingRental?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.RoomOptions)
            .FirstOrDefaultAsync(r => r.ListingId == listingId, cancellationToken);
    }

    public async Task<ListingRentalRoom?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ListingRentalRoom>()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task AddRoomAsync(ListingRentalRoom room, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<ListingRentalRoom>().AddAsync(room, cancellationToken);
    }

    public void RemoveRoom(ListingRentalRoom room)
    {
        DbContext.Set<ListingRentalRoom>().Remove(room);
    }
}
