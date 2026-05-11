using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IListingRentalRepository : IBaseRepository<ListingRental, int>
{
    Task<ListingRental?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default);
    Task<ListingRentalRoom?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddRoomAsync(ListingRentalRoom room, CancellationToken cancellationToken = default);
    void RemoveRoom(ListingRentalRoom room);
}
