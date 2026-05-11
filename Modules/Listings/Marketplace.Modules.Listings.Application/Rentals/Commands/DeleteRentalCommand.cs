using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Commands;

public sealed record DeleteRentalCommand(int ListingId) : IRequest<Unit>;

internal sealed class DeleteRentalHandler(IListingRentalService service)
    : IRequestHandler<DeleteRentalCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRentalCommand request, CancellationToken cancellationToken)
    {
        await service.DeleteByListingIdAsync(request.ListingId, cancellationToken);
        return Unit.Value;
    }
}
