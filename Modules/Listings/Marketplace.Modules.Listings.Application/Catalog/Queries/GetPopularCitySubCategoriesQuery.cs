using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetPopularCitySubCategoriesQuery(int CityId, int Take = 12)
    : IRequest<IReadOnlyList<CatalogSubCategoryDto>>;

internal sealed class GetPopularCitySubCategoriesHandler(ICatalogDataService data)
    : IRequestHandler<GetPopularCitySubCategoriesQuery, IReadOnlyList<CatalogSubCategoryDto>>
{
    public async Task<IReadOnlyList<CatalogSubCategoryDto>> Handle(
        GetPopularCitySubCategoriesQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetPopularCitySubCategoriesAsync(request.CityId, cancellationToken, request.Take);
        return items.Select(x => CatalogDtoMapper.ToDto(x.SubCategory, x.ListingsCount)).ToList();
    }
}
