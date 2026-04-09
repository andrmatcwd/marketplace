using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.CreateLocation;

public sealed class CreateLocationHandler : IRequestHandler<CreateLocationCommand, int>
{
    public Task<int> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
