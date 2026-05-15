namespace Marketplace.Web.Models.Vacancies;

public sealed class VacancyCardVm
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EmploymentType { get; set; }
    public string? SalaryText { get; set; }
    public string? LocationText { get; set; }

    public string ListingTitle { get; set; } = string.Empty;
    public string ListingUrl { get; set; } = string.Empty;

    public string CityName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string SubCategoryName { get; set; } = string.Empty;
}
