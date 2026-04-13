using System;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Models.Listings;

public class ListingsFilter : PaginatonFilter
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public string? Subcategory { get; set; }
    public string? City { get; set; }
}
