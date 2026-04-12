namespace Marketplace.Modules.Listings.Application.Cities.Dtos;

public class CityDto
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
