using System;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Services;

public class ListingService : IListingService
{
    public Task<ListingDetailsPageVm?> GetListingDetailsPageAsync(string city, string categorySlug, string subCategorySlug, string listingSlug, int listingId, BaseFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
