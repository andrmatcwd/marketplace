using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetRegionsByFilter;

public sealed class GetRegionsByFilterHandler
    : IRequestHandler<GetRegionsByFilterQuery, PagedResult<RegionDto>>
{
    private readonly IRegionService _regionService;

    public GetRegionsByFilterHandler(IRegionService regionService)
    {
        _regionService = regionService;
    }
    
    public Task<PagedResult<RegionDto>> Handle(GetRegionsByFilterQuery request, CancellationToken cancellationToken)
    {
        return _regionService.GetByFilterAsync(request.Filter, cancellationToken);
    }
}
