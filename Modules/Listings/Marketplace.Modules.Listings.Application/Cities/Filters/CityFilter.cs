namespace Marketplace.Modules.Listings.Application.Cities.Filters;

public sealed class CityFilter
{
    public string? Search { get; set; }
    public int? RegionId { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
