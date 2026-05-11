using Marketplace.Modules.Listings.Application.Vacancies.Commands;
using Marketplace.Modules.Listings.Application.Vacancies.Dtos;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IListingVacancyService
{
    Task<IReadOnlyList<VacancyDto>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken);
    Task<VacancyDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(CreateVacancyCommand command, CancellationToken cancellationToken);
    Task EditAsync(EditVacancyCommand command, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
