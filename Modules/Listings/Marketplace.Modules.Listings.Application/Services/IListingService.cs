using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Filters;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IListingService
{
    Task<ListingDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<ListingDto> GetBySlagsAsync(
        string citySlag,
        string categorySlag,
        string subCategorySlag,
        string slag,
        int id,
        CancellationToken cancellationToken
    );

    Task<PagedResult<ListingDto>> GetByFilterAsync(ListingFilter filter, CancellationToken cancellationToken);

    Task AddAsync(CreateListingCommand command, CancellationToken cancellationToken);

    Task EditAsync(EditListingCommand command, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
