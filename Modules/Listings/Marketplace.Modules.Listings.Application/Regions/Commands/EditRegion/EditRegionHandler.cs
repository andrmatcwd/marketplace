using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;

public sealed class EditRegionHandler : IRequestHandler<EditRegionCommand, int>
{
    public Task<int> Handle(EditRegionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
