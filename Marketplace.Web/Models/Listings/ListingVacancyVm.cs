namespace Marketplace.Web.Models.Listings;

public sealed class ListingVacancyVm
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? EmploymentType { get; set; }
    public string? SalaryText { get; set; }
    public string? LocationText { get; set; }
}