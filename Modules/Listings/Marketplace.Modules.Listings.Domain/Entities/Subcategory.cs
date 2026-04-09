namespace Marketplace.Modules.Listings.Domain.Entities;

public class SubCategory
{
    public int Id { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
