namespace Marketplace.Modules.Listings.Application.Locations.Dtos;

public class LocationDto
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Domain.Enums.LocationType Type { get; set; }
}
