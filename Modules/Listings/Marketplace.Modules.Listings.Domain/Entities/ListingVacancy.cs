namespace Marketplace.Modules.Listings.Domain.Entities;

public class ListingVacancy
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EmploymentType { get; set; }
    public string? SalaryText { get; set; }
    public string? LocationText { get; set; }
}
