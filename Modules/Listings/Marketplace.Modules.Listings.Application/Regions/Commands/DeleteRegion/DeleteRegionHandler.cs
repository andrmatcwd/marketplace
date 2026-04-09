using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.DeleteRegion;

public sealed class DeleteRegionHandler : IRequestHandler<DeleteRegionCommand, int>
{
    public Task<int> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
