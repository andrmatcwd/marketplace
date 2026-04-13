using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.SubCategories.Filters;

public sealed class SubCategoryFilter : PaginationFilter
{
    public string? CitySlug { get; set; }
    public string? CategorySlug { get; set; }
}
