using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetById;

public sealed class GetRegionByIdHandler
    : IRequestHandler<GetRegionByIdQuery, RegionDto>
{
    private readonly IRegionService _regionService;

    public GetRegionByIdHandler(IRegionService regionService)
    {
        _regionService = regionService;
    }
    
    public Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
    {
        return _regionService.GetByIdAsync(request.Id, cancellationToken);
    }
}
