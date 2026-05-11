using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCategorySubCategoriesInCityQuery(int CityId, int CategoryId)
    : IRequest<IReadOnlyList<CatalogSubCategoryDto>>;

internal sealed class GetCategorySubCategoriesInCityHandler(ICatalogDataService data)
    : IRequestHandler<GetCategorySubCategoriesInCityQuery, IReadOnlyList<CatalogSubCategoryDto>>
{
    public async Task<IReadOnlyList<CatalogSubCategoryDto>> Handle(
        GetCategorySubCategoriesInCityQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetCategorySubCategoriesInCityAsync(request.CityId, request.CategoryId, cancellationToken);
        return items.Select(x => CatalogDtoMapper.ToDto(x.SubCategory, x.ListingsCount)).ToList();
    }
}
