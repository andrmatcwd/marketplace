using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogCityBySlugQuery(string Slug) : IRequest<CatalogCityDto?>;

internal sealed class GetCatalogCityBySlugHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogCityBySlugQuery, CatalogCityDto?>
{
    public async Task<CatalogCityDto?> Handle(
        GetCatalogCityBySlugQuery request, CancellationToken cancellationToken)
    {
        var city = await data.GetPublishedCityBySlugAsync(request.Slug, cancellationToken);
        return city is null ? null : CatalogDtoMapper.ToDto(city, 0);
    }
}
