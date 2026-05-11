namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingVacancyDto(
    string Title,
    string? Description,
    string? EmploymentType,
    string? SalaryText,
    string? LocationText);
