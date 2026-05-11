namespace Marketplace.Web.Areas.Admin.Models.Vacancies;

public class VacancyListItemVm
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? EmploymentType { get; set; }
    public string? SalaryText { get; set; }
    public string? LocationText { get; set; }
}
