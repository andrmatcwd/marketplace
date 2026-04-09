using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;

public sealed class CreateRegionHandler : IRequestHandler<CreateRegionCommand, int>
{
    public Task<int> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
