namespace Marketplace.Modules.Listings.Application.Vacancies.Dtos;

public class VacancyDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EmploymentType { get; set; }
    public string? SalaryText { get; set; }
    public string? LocationText { get; set; }
}
