using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Categories.Filters;

public sealed class CategoryFilter : PaginationFilter
{
    public string? CitySlug { get; set; }
}
