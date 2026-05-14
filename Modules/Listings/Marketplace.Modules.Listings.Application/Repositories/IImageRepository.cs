using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IImageRepository : IBaseRepository<Image, int>
{
    Task<IReadOnlyList<Image>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default);
}
