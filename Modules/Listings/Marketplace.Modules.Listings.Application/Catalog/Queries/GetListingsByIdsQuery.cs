using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetListingsByIdsQuery(IReadOnlyList<int> Ids)
    : IRequest<IReadOnlyList<ListingCardDto>>;

internal sealed class GetListingsByIdsHandler(ICatalogDataService data)
    : IRequestHandler<GetListingsByIdsQuery, IReadOnlyList<ListingCardDto>>
{
    public async Task<IReadOnlyList<ListingCardDto>> Handle(
        GetListingsByIdsQuery request, CancellationToken cancellationToken)
    {
        var tasks = request.Ids
            .Select(id => data.GetPublishedListingByIdAsync(id, cancellationToken));

        var listings = await Task.WhenAll(tasks);

        return listings
            .Where(l => l is not null)
            .Select(l => CatalogDtoMapper.ToCardDto(l!))
            .ToList();
    }
}
