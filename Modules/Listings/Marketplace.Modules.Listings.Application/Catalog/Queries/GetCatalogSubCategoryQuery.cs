using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCatalogSubCategoryQuery(string CategorySlug, string SubCategorySlug)
    : IRequest<CatalogSubCategoryDto?>;

internal sealed class GetCatalogSubCategoryHandler(ICatalogDataService data)
    : IRequestHandler<GetCatalogSubCategoryQuery, CatalogSubCategoryDto?>
{
    public async Task<CatalogSubCategoryDto?> Handle(
        GetCatalogSubCategoryQuery request, CancellationToken cancellationToken)
    {
        var sub = await data.GetPublishedSubCategoryWithCategoryAsync(
            request.CategorySlug, request.SubCategorySlug, cancellationToken);
        return sub is null ? null : CatalogDtoMapper.ToDto(sub, 0);
    }
}
