namespace Marketplace.Web.Models.Listings;

public sealed class ListingItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string City { get; set; } = string.Empty;

    public string? AddressLine { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public double Rating { get; set; }
    public bool IsOnline { get; set; }
    public bool IsOffline { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public List<string> ImageUrls { get; set; } = [];
}