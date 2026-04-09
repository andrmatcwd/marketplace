using Marketplace.Modules.Listings.Application.Regions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetById;

public sealed class GetRegionByIdHandler : IRequestHandler<GetRegionByIdQuery, RegionDto>
{
    public Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new RegionDto());
    }
}
