using Marketplace.Modules.Listings.Application.Regions.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IRegionRepository : IBaseRepository<Region, int>
{
    Task<(IReadOnlyCollection<Region> Items, int TotalCount)> GetByFilterAsync(
        RegionFilter filter,
        CancellationToken cancellationToken = default);
}
