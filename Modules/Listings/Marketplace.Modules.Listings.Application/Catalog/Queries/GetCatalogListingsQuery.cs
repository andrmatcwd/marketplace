using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogListingsQuery(CatalogListingFilter Filter)
    : IRequest<IReadOnlyList<ListingCardDto>>;

internal sealed class GetCatalogListingsHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogListingsQuery, IReadOnlyList<ListingCardDto>>
{
    public async Task<IReadOnlyList<ListingCardDto>> Handle(
        GetCatalogListingsQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetPublishedListingsAsync(request.Filter, cancellationToken);
        return items.Select(CatalogDtoMapper.ToCardDto).ToList();
    }
}
