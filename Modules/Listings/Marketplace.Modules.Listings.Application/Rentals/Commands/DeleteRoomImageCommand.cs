using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record DeleteRoomImageCommand(int RoomId, string Url) : IRequest<string?>;

internal sealed class DeleteRoomImageHandler(IListingRentalService service)
    : IRequestHandler<DeleteRoomImageCommand, string?>
{
    public Task<string?> Handle(DeleteRoomImageCommand request, CancellationToken cancellationToken)
        => service.DeleteRoomImageAsync(request.RoomId, request.Url, cancellationToken);
}
