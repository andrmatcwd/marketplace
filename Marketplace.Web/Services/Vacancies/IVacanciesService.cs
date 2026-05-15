using Marketplace.Web.Models.Vacancies;

namespace Marketplace.Web.Services.Vacancies;

public interface IVacanciesService
{
    Task<VacanciesPageVm> GetVacanciesPageAsync(
        string culture,
        VacanciesFilterVm filter,
        CancellationToken cancellationToken);
}
