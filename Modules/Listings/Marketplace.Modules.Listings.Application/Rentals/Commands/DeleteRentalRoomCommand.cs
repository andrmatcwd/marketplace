using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record DeleteRentalRoomCommand(int Id) : IRequest<Unit>;

internal sealed class DeleteRentalRoomHandler(IListingRentalService service)
    : IRequestHandler<DeleteRentalRoomCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRentalRoomCommand request, CancellationToken cancellationToken)
    {
        await service.DeleteRoomAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
