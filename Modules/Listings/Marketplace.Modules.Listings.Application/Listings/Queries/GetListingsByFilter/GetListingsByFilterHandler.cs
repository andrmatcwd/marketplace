using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;

public sealed class GetListingsByFilterHandler
    : IRequestHandler<GetListingsByFilterQuery, PagedResult<ListingDto>>
{
    private readonly IListingService _listingService;

    public GetListingsByFilterHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public Task<PagedResult<ListingDto>> Handle(GetListingsByFilterQuery request, CancellationToken cancellationToken)
    {
        return _listingService.GetByFilterAsync(request.Filter, cancellationToken);
    }
}

