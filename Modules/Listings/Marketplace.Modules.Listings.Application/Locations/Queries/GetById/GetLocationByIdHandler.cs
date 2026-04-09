using Marketplace.Modules.Listings.Application.Locations.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Queries.GetById;

public sealed class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, LocationDto>
{
    public Task<LocationDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new LocationDto());
    }
}
