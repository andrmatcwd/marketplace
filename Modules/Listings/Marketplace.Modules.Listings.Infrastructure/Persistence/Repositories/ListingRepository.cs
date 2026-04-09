using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ListingRepository
    : BaseRepository<Listing, Guid>, IListingRepository
{
    public ListingRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
