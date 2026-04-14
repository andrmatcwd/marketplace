namespace Marketplace.Web.Models.Cards;

public sealed class CategoryCardVm
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public int ListingsCount { get; set; }
}