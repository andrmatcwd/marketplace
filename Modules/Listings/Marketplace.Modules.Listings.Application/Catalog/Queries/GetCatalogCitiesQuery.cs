using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogCitiesQuery(int? Take = null) : IRequest<IReadOnlyList<CatalogCityDto>>;

internal sealed class GetCatalogCitiesHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogCitiesQuery, IReadOnlyList<CatalogCityDto>>
{
    public async Task<IReadOnlyList<CatalogCityDto>> Handle(
        GetCatalogCitiesQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetPublishedCitiesAsync(cancellationToken, request.Take);
        return items.Select(x => CatalogDtoMapper.ToDto(x.City, x.ListingsCount)).ToList();
    }
}
