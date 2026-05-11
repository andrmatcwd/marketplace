using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCityCatalogCategoriesQuery(int CityId) : IRequest<IReadOnlyList<CatalogCategoryDto>>;

internal sealed class GetCityCatalogCategoriesHandler(ICatalogDataService data)
    : IRequestHandler<GetCityCatalogCategoriesQuery, IReadOnlyList<CatalogCategoryDto>>
{
    public async Task<IReadOnlyList<CatalogCategoryDto>> Handle(
        GetCityCatalogCategoriesQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetCityCategoriesAsync(request.CityId, cancellationToken);
        return items.Select(x => CatalogDtoMapper.ToDto(x.Category, x.ListingsCount)).ToList();
    }
}
