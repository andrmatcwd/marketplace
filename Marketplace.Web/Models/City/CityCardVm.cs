using System;

namespace Marketplace.Web.Models.City;

public class CityCardVm
{
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;

    public string RegionName { get; set; } = string.Empty;
    public string RegionSlug { get; set; } = string.Empty;

    public int ListingsCount { get; set; }
    public int CategoriesCount { get; set; }

    public string Url { get; init; } = default!;
}
