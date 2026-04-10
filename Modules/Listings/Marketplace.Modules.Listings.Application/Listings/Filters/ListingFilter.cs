using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Listings.Filters;

public class ListingFilter : PaginationFilter
{
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public bool? IsActive { get; init; }
    public int? LocationId { get; init; }
    public string? OrderBy { get; init; }
}
