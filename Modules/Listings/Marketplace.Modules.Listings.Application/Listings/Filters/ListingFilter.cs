using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Domain.Enums.Listing;

namespace Marketplace.Modules.Listings.Application.Listings.Filters;

public class ListingFilter : PaginationFilter
{
    public string? CitySlug { get; set; }
    public string? CategorySlug { get; set; }
    public string? SubCategorySlug { get; set; }
    public int? CityId { get; set; }
    public int? CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public ListingStatus? Status { get; set; }
    public string? SellerId { get; set; }
}
