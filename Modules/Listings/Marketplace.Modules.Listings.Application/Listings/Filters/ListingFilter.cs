using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Listings.Filters;

public class ListingFilter : PaginationFilter
{
    public string? CitySlug { get; set; }
    public string? CategorySlug { get; set; }
    public string? SubCategorySlug { get; set; }
}
