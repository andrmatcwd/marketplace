using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IListingRepository : IBaseRepository<Listing, int>
{
    Task<(IReadOnlyCollection<Listing> Items, int TotalCount)> GetListingsAsync(
        ListingFilter filter,
        CancellationToken cancellationToken = default);
}
