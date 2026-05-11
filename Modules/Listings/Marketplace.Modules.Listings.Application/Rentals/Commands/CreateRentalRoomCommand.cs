using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record CreateRentalRoomCommand(
    int RentalId,
    string Title,
    string? Description,
    string? Price,
    string? Area,
    string? Guests,
    string? Beds,
    List<string> Amenities,
    List<string> ImageUrls) : IRequest<Unit>;

internal sealed class CreateRentalRoomHandler(IListingRentalService service)
    : IRequestHandler<CreateRentalRoomCommand, Unit>
{
    public async Task<Unit> Handle(CreateRentalRoomCommand request, CancellationToken cancellationToken)
    {
        await service.AddRoomAsync(request, cancellationToken);
        return Unit.Value;
    }
}
