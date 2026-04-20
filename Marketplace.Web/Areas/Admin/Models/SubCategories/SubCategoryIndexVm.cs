namespace Marketplace.Web.Areas.Admin.Models.SubCategories;

public class SubCategoryIndexVm
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public IReadOnlyCollection<SubCategoryListItemVm> Items { get; init; } = [];
}