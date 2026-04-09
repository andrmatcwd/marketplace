using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;

public sealed class GetListingsByFilterHandler
: IRequestHandler<GetListingsByFilterQuery, PagedResult<ListingDto>>
{
    private readonly IListingService listingService;

    public GetListingsByFilterHandler(IListingService listingService)
    {
        this.listingService = listingService;
    }

    public async Task<PagedResult<ListingDto>> Handle(GetListingsByFilterQuery request, CancellationToken cancellationToken)
    {
        var result = await listingService.GetListingsAsync(request.Filter, cancellationToken);

        return result;
    }
}

