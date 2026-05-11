using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record CountCatalogListingsQuery(CatalogListingFilter Filter) : IRequest<int>;

internal sealed class CountCatalogListingsHandler(ICatalogDataService data)
    : IRequestHandler<CountCatalogListingsQuery, int>
{
    public Task<int> Handle(CountCatalogListingsQuery request, CancellationToken cancellationToken)
        => data.CountPublishedListingsAsync(request.Filter, cancellationToken);
}
