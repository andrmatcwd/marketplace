using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Vacancies;

public sealed class VacanciesFilterVm
{
    public string? Search { get; set; }
    public string? City { get; set; }
    public string? EmploymentType { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public string ResetUrl { get; set; } = "/vacancies";

    public IReadOnlyCollection<FilterOptionVm> Cities { get; set; } = Array.Empty<FilterOptionVm>();
    public IReadOnlyCollection<FilterOptionVm> EmploymentTypes { get; set; } = Array.Empty<FilterOptionVm>();
}
