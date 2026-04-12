namespace Marketplace.Modules.Listings.Application.SubCategories.Filters;

public sealed class SubCategoryFilter
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
