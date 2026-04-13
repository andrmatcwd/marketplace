using System;

namespace Marketplace.Web.Models.Category;

public class CategoryCardVm
{
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;

    public string? Description { get; set; }
    public string? Icon { get; set; }

    public int SubCategoryCount { get; init; }
    public int ListingsCount { get; init; }

    public string Url { get; init; } = default!;
}
