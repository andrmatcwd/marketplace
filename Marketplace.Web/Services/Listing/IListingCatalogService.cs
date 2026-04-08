using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Services.Listing;

public interface IListingCatalogService
{
    Task<IReadOnlyList<ListingCategoryViewModel>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<PagedResult<ListingItemViewModel>> GetListingsAsync(
        ListingsFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<ListingItemViewModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        ListingItemViewModel model,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ListingItemViewModel model,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}