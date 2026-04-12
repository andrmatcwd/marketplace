namespace Marketplace.Modules.Listings.Application.Categories.Filters;

public sealed class CategoryFilter
{
    public string? Search { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
