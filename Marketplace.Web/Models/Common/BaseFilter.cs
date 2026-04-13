using System;

namespace Marketplace.Web.Models.Common;

public class BaseFilter
{
    public string? SearchTerm { get; set; }

    public string SortBy { get; set; } = "newest";
}
