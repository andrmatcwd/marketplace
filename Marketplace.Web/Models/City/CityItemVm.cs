using System;

namespace Marketplace.Web.Models.City;

public class CityItemVm
{
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public int ListingsCount { get; init; }
    public string Url { get; init; } = default!;
}
