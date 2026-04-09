namespace Marketplace.Modules.Listings.Domain.Entities;

public class Region
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<Location> Locations { get; set; } = new List<Location>();
}
