namespace Marketplace.Modules.Listings.Application.Cities.Dtos;

public class CityDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string RegionName { get; set; } = string.Empty;
    public string RegionSlug { get; set; } = string.Empty;

    public int ListingsCount { get; set; }
    public int CategoriesCount { get; set; }
}
