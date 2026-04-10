using System;

namespace Marketplace.Web.Models.Common;

public class BaseFilter
{
    public string? SearchTerm { get; set; }

    public double? RatingFrom { get; set; }
    public string SortBy { get; set; } = "newest";

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
