using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetFeaturedListingsQuery(int Take = 8) : IRequest<IReadOnlyList<ListingCardDto>>;

internal sealed class GetFeaturedListingsHandler(ICatalogDataService data)
    : IRequestHandler<GetFeaturedListingsQuery, IReadOnlyList<ListingCardDto>>
{
    public async Task<IReadOnlyList<ListingCardDto>> Handle(
        GetFeaturedListingsQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetFeaturedListingsAsync(request.Take, cancellationToken);
        return items.Select(CatalogDtoMapper.ToCardDto).ToList();
    }
}
