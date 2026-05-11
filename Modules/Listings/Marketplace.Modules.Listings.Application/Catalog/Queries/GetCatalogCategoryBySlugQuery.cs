using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogCategoryBySlugQuery(string Slug) : IRequest<CatalogCategoryDto?>;

internal sealed class GetCatalogCategoryBySlugHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogCategoryBySlugQuery, CatalogCategoryDto?>
{
    public async Task<CatalogCategoryDto?> Handle(
        GetCatalogCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await data.GetPublishedCategoryBySlugAsync(request.Slug, cancellationToken);
        return category is null ? null : CatalogDtoMapper.ToDto(category, 0);
    }
}
