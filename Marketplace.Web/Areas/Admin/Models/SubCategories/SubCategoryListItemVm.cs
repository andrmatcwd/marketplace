namespace Marketplace.Web.Areas.Admin.Models.SubCategories;

public class SubCategoryListItemVm
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int ListingsCount { get; init; }
}