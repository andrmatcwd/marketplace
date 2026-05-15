using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetListingMapMarkersQuery(
    int CityId,
    int? CategoryId,
    int? SubCategoryId)
    : IRequest<IReadOnlyList<ListingMapMarkerDto>>;

internal sealed class GetListingMapMarkersHandler(ICatalogDataService data)
    : IRequestHandler<GetListingMapMarkersQuery, IReadOnlyList<ListingMapMarkerDto>>
{
    public Task<IReadOnlyList<ListingMapMarkerDto>> Handle(
        GetListingMapMarkersQuery request, CancellationToken cancellationToken)
        => data.GetListingMapMarkersAsync(request.CityId, request.CategoryId, request.SubCategoryId, cancellationToken);
}
