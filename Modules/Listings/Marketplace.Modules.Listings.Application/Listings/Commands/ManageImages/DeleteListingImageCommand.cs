using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.ManageImages;

public sealed record DeleteListingImageCommand(int ImageId) : IRequest<string?>;

internal sealed class DeleteListingImageHandler(IImageService service)
    : IRequestHandler<DeleteListingImageCommand, string?>
{
    public Task<string?> Handle(DeleteListingImageCommand request, CancellationToken cancellationToken)
        => service.DeleteAsync(request.ImageId, cancellationToken);
}
