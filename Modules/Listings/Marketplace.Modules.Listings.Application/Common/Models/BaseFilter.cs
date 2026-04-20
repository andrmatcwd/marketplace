using System;

namespace Marketplace.Modules.Listings.Application.Common.Models;

public class BaseFilter
{
    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; }
}
