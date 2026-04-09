using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetRegionsByFilter;

public sealed class GetRegionsByFilterQueryHandler : IRequestHandler<GetRegionsByFilterQuery, PagedResult<RegionDto>>
{
    public Task<PagedResult<RegionDto>> Handle(GetRegionsByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<RegionDto>());
    }
}
