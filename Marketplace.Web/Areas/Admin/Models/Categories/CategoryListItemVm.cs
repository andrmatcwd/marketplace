namespace Marketplace.Web.Areas.Admin.Models.Categories;

public class CategoryListItemVm
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int SubCategoriesCount { get; init; }
    public int ListingsCount { get; init; }
}