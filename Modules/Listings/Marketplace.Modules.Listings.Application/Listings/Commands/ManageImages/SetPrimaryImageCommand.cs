using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.ManageImages;

public sealed record SetPrimaryImageCommand(int ListingId, int ImageId) : IRequest<Unit>;

internal sealed class SetPrimaryImageHandler(IImageService service)
    : IRequestHandler<SetPrimaryImageCommand, Unit>
{
    public async Task<Unit> Handle(SetPrimaryImageCommand request, CancellationToken cancellationToken)
    {
        await service.SetPrimaryAsync(request.ListingId, request.ImageId, cancellationToken);
        return Unit.Value;
    }
}
