namespace Marketplace.Web.Models.Services;

public sealed class ServiceItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string City { get; set; } = string.Empty;
    public double Rating { get; set; }
    public bool IsOnline { get; set; }
    public bool IsOffline { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}