namespace Marketplace.Web.Models.Services;

public sealed class ServicesFilterRequest
{
    public string? Search { get; set; }
    public List<string> Categories { get; set; } = [];
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public bool OnlineOnly { get; set; }
    public bool OfflineOnly { get; set; }
    public double? RatingFrom { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}