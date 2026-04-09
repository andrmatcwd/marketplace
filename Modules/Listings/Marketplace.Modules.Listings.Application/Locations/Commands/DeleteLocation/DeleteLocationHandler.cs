using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.DeleteLocation;

public sealed class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, int>
{
    public Task<int> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
