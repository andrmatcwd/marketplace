using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record SaveRentalCommand(
    int ListingId,
    string? Price,
    string? Rooms,
    string? Area,
    string? Floor,
    List<string> Features) : IRequest<Unit>;

internal sealed class SaveRentalHandler(IListingRentalService service)
    : IRequestHandler<SaveRentalCommand, Unit>
{
    public async Task<Unit> Handle(SaveRentalCommand request, CancellationToken cancellationToken)
    {
        await service.SaveAsync(request, cancellationToken);
        return Unit.Value;
    }
}
