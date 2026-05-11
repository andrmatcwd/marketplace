using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetListingDetailsQuery(int Id) : IRequest<ListingDetailsDto?>;

internal sealed class GetListingDetailsHandler(ICatalogDataService data)
    : IRequestHandler<GetListingDetailsQuery, ListingDetailsDto?>
{
    public async Task<ListingDetailsDto?> Handle(
        GetListingDetailsQuery request, CancellationToken cancellationToken)
    {
        var listing = await data.GetPublishedListingByIdAsync(request.Id, cancellationToken);
        return listing is null ? null : CatalogDtoMapper.ToDetailsDto(listing);
    }
}
