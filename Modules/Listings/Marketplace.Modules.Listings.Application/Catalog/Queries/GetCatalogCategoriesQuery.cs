using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogCategoriesQuery(int? Take = null) : IRequest<IReadOnlyList<CatalogCategoryDto>>;

internal sealed class GetCatalogCategoriesHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogCategoriesQuery, IReadOnlyList<CatalogCategoryDto>>
{
    public async Task<IReadOnlyList<CatalogCategoryDto>> Handle(
        GetCatalogCategoriesQuery request, CancellationToken cancellationToken)
    {
        var items = await data.GetPublishedCategoriesAsync(cancellationToken, request.Take);
        return items.Select(x => CatalogDtoMapper.ToDto(x.Category, x.ListingsCount)).ToList();
    }
}
