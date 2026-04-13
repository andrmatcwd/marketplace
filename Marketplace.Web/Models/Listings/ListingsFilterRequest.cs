namespace Marketplace.Web.Models.Listings;

public sealed class ListingsFilterRequest
{
    public string? Search { get; set; }
    public List<string> Categories { get; set; } = [];
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public bool OnlineOnly { get; set; }
    public bool OfflineOnly { get; set; }
    public string? City { get; set; }
    public double? RatingFrom { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}