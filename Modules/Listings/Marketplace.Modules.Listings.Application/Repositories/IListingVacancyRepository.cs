using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface IListingVacancyRepository : IBaseRepository<ListingVacancy, int>
{
    Task<IReadOnlyList<ListingVacancy>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default);
}
