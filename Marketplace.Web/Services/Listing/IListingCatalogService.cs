using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Services.Listing;

public interface IListingCatalogService
{
    Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<PagedResult<ListingViewModel>> GetListingsAsync(
        ListingsFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<ListingViewModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        ListingViewModel model,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ListingViewModel model,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}