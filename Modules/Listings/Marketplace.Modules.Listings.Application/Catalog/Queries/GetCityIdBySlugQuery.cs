using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetCityIdBySlugQuery(string CitySlug) : IRequest<int?>;

internal sealed class GetCityIdBySlugHandler(ICatalogDataService data)
    : IRequestHandler<GetCityIdBySlugQuery, int?>
{
    public Task<int?> Handle(GetCityIdBySlugQuery request, CancellationToken cancellationToken)
        => data.GetCityIdBySlugAsync(request.CitySlug, cancellationToken);
}
