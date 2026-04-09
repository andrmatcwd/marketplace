using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class LocationRepository
    : BaseRepository<Location, int>, ILocationRepository
{
    public LocationRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
