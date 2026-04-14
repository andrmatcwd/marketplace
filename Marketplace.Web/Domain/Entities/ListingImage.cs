namespace Marketplace.Web.Domain.Entities;

public sealed class ListingImage
{
    public Guid Id { get; set; }

    public Guid ListingId { get; set; }
    public Listing? Listing { get; set; }

    public string Url { get; set; } = string.Empty;
    public string? Alt { get; set; }

    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
}