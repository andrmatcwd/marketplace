using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetSitemapDataQuery : IRequest<SitemapDataDto>;

internal sealed class GetSitemapDataHandler(ICatalogDataService data)
    : IRequestHandler<GetSitemapDataQuery, SitemapDataDto>
{
    public Task<SitemapDataDto> Handle(GetSitemapDataQuery request, CancellationToken cancellationToken)
        => data.GetSitemapDataAsync(cancellationToken);
}
