using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Services.Listings;

public interface IListingService
{
    Task<ListingDetailsPageVm?> GetDetailsPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        int id,
        CancellationToken cancellationToken);
}
