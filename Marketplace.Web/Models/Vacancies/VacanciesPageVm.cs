using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Vacancies;

public sealed class VacanciesPageVm
{
    public string Culture { get; set; } = "uk";
    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();
    public VacanciesFilterVm Filter { get; set; } = new();
    public IReadOnlyList<VacancyCardVm> Vacancies { get; set; } = Array.Empty<VacancyCardVm>();
    public PaginationVm? Pagination { get; set; }
    public int TotalCount { get; set; }
    public bool HasVacancies => Vacancies.Count > 0;
}
