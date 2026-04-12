using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetById;

public sealed class GetListingByIdHandler
    : IRequestHandler<GetListingByIdQuery, ListingDto>
{
    private readonly IListingService _listingService;

    public GetListingByIdHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public Task<ListingDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        return _listingService.GetByIdAsync(request.Id, cancellationToken);
    }
}

