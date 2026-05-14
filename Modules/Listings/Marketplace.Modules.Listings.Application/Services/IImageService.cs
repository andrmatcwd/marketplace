using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IImageService
{
    Task<int> AddAsync(int listingId, string url, string? alt, CancellationToken cancellationToken = default);
    Task<string?> DeleteAsync(int imageId, CancellationToken cancellationToken = default);
    Task SetPrimaryAsync(int listingId, int imageId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Image>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default);
}
