namespace Marketplace.Modules.Listings.Application.Categories.Dtos;

public class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? Icon { get; set; }

    public string CityName { get; set; } = string.Empty;
    public string CitySlug { get; set; } = string.Empty;

    public int ListingsCount { get; set; }
    public int SubCategoriesCount { get; set; }
}
