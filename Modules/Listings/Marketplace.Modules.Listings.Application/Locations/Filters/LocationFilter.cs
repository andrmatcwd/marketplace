namespace Marketplace.Modules.Listings.Application.Locations.Filters;

public sealed class LocationFilter
{
    public int? RegionId { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public Marketplace.Modules.Listings.Domain.Enums.LocationType? Type { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
