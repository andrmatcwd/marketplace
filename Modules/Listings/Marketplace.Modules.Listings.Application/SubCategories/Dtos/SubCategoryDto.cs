namespace Marketplace.Modules.Listings.Application.SubCategories.Dtos;

public class SubCategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;

    public int ListingsCount { get; set; }
}
