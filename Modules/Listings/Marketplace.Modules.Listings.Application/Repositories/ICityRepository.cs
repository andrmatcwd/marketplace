using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface ICityRepository : IBaseRepository<City, int>
{
    Task<(IReadOnlyCollection<City> Items, int TotalCount)> GetByFilterAsync(
        CityFilter filter,
        CancellationToken cancellationToken = default);
}
