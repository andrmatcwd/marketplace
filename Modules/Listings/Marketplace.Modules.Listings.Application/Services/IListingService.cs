using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Filters;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IListingService
{
    Task<PagedResult<ListingDto>> GetListingsAsync(
        ListingFilter filter,
        CancellationToken cancellationToken = default);
}
