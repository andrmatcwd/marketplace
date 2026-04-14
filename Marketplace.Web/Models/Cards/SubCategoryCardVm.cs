namespace Marketplace.Web.Models.Cards;

public sealed class SubCategoryCardVm
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int ListingsCount { get; set; }
}