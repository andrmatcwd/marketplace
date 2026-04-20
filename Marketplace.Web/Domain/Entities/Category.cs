namespace Marketplace.Web.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }

    public bool IsPublished { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}