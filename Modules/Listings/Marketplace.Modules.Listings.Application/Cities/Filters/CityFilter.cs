using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Cities.Filters;

public sealed class CityFilter : PaginationFilter
{
    public string? RegionsSlug { get; set; }
}
