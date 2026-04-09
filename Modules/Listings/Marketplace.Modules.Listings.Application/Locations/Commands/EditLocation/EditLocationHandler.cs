using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.EditLocation;

public sealed class EditLocationHandler : IRequestHandler<EditLocationCommand, int>
{
    public Task<int> Handle(EditLocationCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
