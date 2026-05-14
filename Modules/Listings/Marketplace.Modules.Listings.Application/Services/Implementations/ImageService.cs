using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ImageService : IImageService
{
    private readonly IImageRepository _repository;

    public ImageService(IImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(int listingId, string url, string? alt, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByListingIdAsync(listingId, cancellationToken);
        var sortOrder = existing.Count > 0 ? existing.Max(i => i.SortOrder) + 1 : 0;

        var image = new Image
        {
            ListingId = listingId,
            Url = url,
            Alt = alt,
            IsPrimary = existing.Count == 0,
            SortOrder = sortOrder
        };

        await _repository.AddAsync(image, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return image.Id;
    }

    public async Task<string?> DeleteAsync(int imageId, CancellationToken cancellationToken = default)
    {
        var image = await _repository.GetByIdAsync(imageId, cancellationToken);
        if (image is null) return null;

        var url = image.Url;
        var wasPrimary = image.IsPrimary;
        var listingId = image.ListingId;

        _repository.Remove(image);
        await _repository.SaveChangesAsync(cancellationToken);

        if (wasPrimary)
        {
            var remaining = await _repository.GetByListingIdAsync(listingId, cancellationToken);
            if (remaining.Count > 0)
            {
                var first = remaining[0];
                first.IsPrimary = true;
                _repository.Update(first);
                await _repository.SaveChangesAsync(cancellationToken);
            }
        }

        return url;
    }

    public async Task SetPrimaryAsync(int listingId, int imageId, CancellationToken cancellationToken = default)
    {
        var images = await _repository.GetByListingIdAsync(listingId, cancellationToken);
        foreach (var img in images)
        {
            img.IsPrimary = img.Id == imageId;
            _repository.Update(img);
        }
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public Task<IReadOnlyList<Image>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default)
    {
        return _repository.GetByListingIdAsync(listingId, cancellationToken);
    }
}
