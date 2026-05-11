using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record CountListingsByCategoryQuery(int CityId, int CategoryId, string? Search) : IRequest<int>;

internal sealed class CountListingsByCategoryHandler(ICatalogDataService data)
    : IRequestHandler<CountListingsByCategoryQuery, int>
{
    public Task<int> Handle(CountListingsByCategoryQuery request, CancellationToken cancellationToken)
        => data.CountListingsByCategoryInCityAsync(request.CityId, request.CategoryId, request.Search, cancellationToken);
}
