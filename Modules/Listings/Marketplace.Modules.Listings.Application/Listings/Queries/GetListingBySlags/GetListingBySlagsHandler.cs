using System;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingBySlags;

public sealed class GetListingBySlagsHandler
    : IRequestHandler<GetListingBySlagsQuery, ListingDto>
{
    private readonly IListingService _listingService;

    public GetListingBySlagsHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public Task<ListingDto> Handle(GetListingBySlagsQuery request, CancellationToken cancellationToken)
    {
        return _listingService.GetBySlagsAsync(
            request.CitySlag,
            request.CategorySlag,
            request.SubCategorySlag,
            request.ListingSlug,
            request.Id,
            cancellationToken);
    }
}
