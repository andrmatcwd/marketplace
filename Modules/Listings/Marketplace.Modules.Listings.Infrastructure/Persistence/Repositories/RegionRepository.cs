using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class RegionRepository
    : BaseRepository<Region, int>, IRegionRepository
{
    public RegionRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }
}
