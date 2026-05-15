namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed class VacancyListingFilter
{
    public string? Search { get; init; }
    public int? CityId { get; init; }
    public string? EmploymentType { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
}
