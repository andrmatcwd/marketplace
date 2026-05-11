using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetRelatedListingsQuery(int ExcludeId, int CityId, int SubCategoryId, int Take = 6)
    : IRequest<IReadOnlyList<ListingCardDto>>;

internal sealed class GetRelatedListingsHandler(ICatalogDataService data)
    : IRequestHandler<GetRelatedListingsQuery, IReadOnlyList<ListingCardDto>>
{
    public async Task<IReadOnlyList<ListingCardDto>> Handle(
        GetRelatedListingsQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetRelatedListingsAsync(
            request.ExcludeId, request.CityId, request.SubCategoryId, request.Take, cancellationToken);
        return items.Select(CatalogDtoMapper.ToCardDto).ToList();
    }
}
