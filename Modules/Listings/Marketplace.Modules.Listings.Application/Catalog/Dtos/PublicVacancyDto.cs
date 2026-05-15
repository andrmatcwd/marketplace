namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record PublicVacancyDto(
    int Id,
    string Title,
    string? Description,
    string? EmploymentType,
    string? SalaryText,
    string? LocationText,
    int ListingId,
    string ListingTitle,
    string ListingSlug,
    string CityName,
    string CitySlug,
    string CategoryName,
    string CategorySlug,
    string SubCategoryName,
    string SubCategorySlug);
