namespace Marketplace.Web.Models.Listings;

public sealed class ListingImageVm
{
    public string Url { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
}