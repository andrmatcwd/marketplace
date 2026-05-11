using Marketplace.Modules.Listings.Application.Rentals.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Queries;

public sealed record GetRentalByListingIdQuery(int ListingId) : IRequest<RentalAdminDto?>;

internal sealed class GetRentalByListingIdHandler(IListingRentalService service)
    : IRequestHandler<GetRentalByListingIdQuery, RentalAdminDto?>
{
    public async Task<RentalAdminDto?> Handle(GetRentalByListingIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByListingIdAsync(request.ListingId, cancellationToken);
    }
}
