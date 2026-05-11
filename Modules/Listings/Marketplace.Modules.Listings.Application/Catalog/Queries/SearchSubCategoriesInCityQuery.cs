using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record SearchSubCategoriesInCityQuery(int CityId, string Search, int Take)
    : IRequest<IReadOnlyList<SubCategorySearchResult>>;

internal sealed class SearchSubCategoriesInCityHandler(ICatalogDataService data)
    : IRequestHandler<SearchSubCategoriesInCityQuery, IReadOnlyList<SubCategorySearchResult>>
{
    public Task<IReadOnlyList<SubCategorySearchResult>> Handle(
        SearchSubCategoriesInCityQuery request, CancellationToken cancellationToken)
        => data.SearchSubCategoriesInCityAsync(request.CityId, request.Search, request.Take, cancellationToken);
}
