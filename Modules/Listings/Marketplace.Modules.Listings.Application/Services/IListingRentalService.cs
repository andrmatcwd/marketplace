using Marketplace.Modules.Listings.Application.Rentals.Commands;
using Marketplace.Modules.Listings.Application.Rentals.Dtos;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IListingRentalService
{
    Task<RentalAdminDto?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken);
    Task SaveAsync(SaveRentalCommand command, CancellationToken cancellationToken);
    Task DeleteByListingIdAsync(int listingId, CancellationToken cancellationToken);
    Task<RentalRoomAdminDto?> GetRoomByIdAsync(int id, CancellationToken cancellationToken);
    Task AddRoomAsync(CreateRentalRoomCommand command, CancellationToken cancellationToken);
    Task EditRoomAsync(EditRentalRoomCommand command, CancellationToken cancellationToken);
    Task DeleteRoomAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetRoomImagesAsync(int roomId, CancellationToken cancellationToken);
    Task AddRoomImageAsync(int roomId, string url, CancellationToken cancellationToken);
    Task<string?> DeleteRoomImageAsync(int roomId, string url, CancellationToken cancellationToken);
}
