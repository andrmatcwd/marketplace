using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Locations.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Queries.GetLocationsByFilter;

public sealed class GetLocationsByFilterQueryHandler : IRequestHandler<GetLocationsByFilterQuery, PagedResult<LocationDto>>
{
    public Task<PagedResult<LocationDto>> Handle(GetLocationsByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<LocationDto>());
    }
}
