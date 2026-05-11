using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetTopListingsByCategoryQuery(int CityId, int CategoryId, string? Search, int Take)
    : IRequest<IReadOnlyList<ListingCardDto>>;

internal sealed class GetTopListingsByCategoryHandler(ICatalogDataService data)
    : IRequestHandler<GetTopListingsByCategoryQuery, IReadOnlyList<ListingCardDto>>
{
    public async Task<IReadOnlyList<ListingCardDto>> Handle(
        GetTopListingsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetTopListingsByCategoryInCityAsync(
            request.CityId, request.CategoryId, request.Search, request.Take, cancellationToken);
        return items.Select(CatalogDtoMapper.ToCardDto).ToList();
    }
}
