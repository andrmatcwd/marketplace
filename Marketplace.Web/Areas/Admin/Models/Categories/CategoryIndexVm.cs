namespace Marketplace.Web.Areas.Admin.Models.Categories;

public class CategoryIndexVm
{
    public string? Search { get; set; }
    public IReadOnlyCollection<CategoryListItemVm> Items { get; init; } = [];
}