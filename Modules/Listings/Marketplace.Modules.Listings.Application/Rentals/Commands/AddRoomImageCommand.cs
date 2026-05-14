using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record AddRoomImageCommand(int RoomId, string Url) : IRequest<Unit>;

internal sealed class AddRoomImageHandler(IListingRentalService service)
    : IRequestHandler<AddRoomImageCommand, Unit>
{
    public async Task<Unit> Handle(AddRoomImageCommand request, CancellationToken cancellationToken)
    {
        await service.AddRoomImageAsync(request.RoomId, request.Url, cancellationToken);
        return Unit.Value;
    }
}
