using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record EditRentalRoomCommand(
    int Id,
    string Title,
    string? Description,
    string? Price,
    string? Area,
    string? Guests,
    string? Beds,
    List<string> Amenities,
    List<string> ImageUrls) : IRequest<Unit>;

internal sealed class EditRentalRoomHandler(IListingRentalService service)
    : IRequestHandler<EditRentalRoomCommand, Unit>
{
    public async Task<Unit> Handle(EditRentalRoomCommand request, CancellationToken cancellationToken)
    {
        await service.EditRoomAsync(request, cancellationToken);
        return Unit.Value;
    }
}
