using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.ManageImages;

public sealed record AddListingImageCommand(int ListingId, string Url, string? Alt) : IRequest<int>;

internal sealed class AddListingImageHandler(IImageService service)
    : IRequestHandler<AddListingImageCommand, int>
{
    public Task<int> Handle(AddListingImageCommand request, CancellationToken cancellationToken)
        => service.AddAsync(request.ListingId, request.Url, request.Alt, cancellationToken);
}
