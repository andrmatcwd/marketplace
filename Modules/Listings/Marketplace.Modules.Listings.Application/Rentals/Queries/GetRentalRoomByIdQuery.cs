using Marketplace.Modules.Listings.Application.Rentals.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Rentals.Queries;

public sealed record GetRentalRoomByIdQuery(int Id) : IRequest<RentalRoomAdminDto?>;

internal sealed class GetRentalRoomByIdHandler(IListingRentalService service)
    : IRequestHandler<GetRentalRoomByIdQuery, RentalRoomAdminDto?>
{
    public async Task<RentalRoomAdminDto?> Handle(GetRentalRoomByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetRoomByIdAsync(request.Id, cancellationToken);
    }
}
