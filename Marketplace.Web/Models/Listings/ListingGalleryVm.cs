namespace Marketplace.Web.Models.Listings;

public sealed class ListingGalleryVm
{
    public IReadOnlyCollection<ListingImageVm> Images { get; set; } = Array.Empty<ListingImageVm>();

    public bool HasImages => Images.Count > 0;

    public string GetPrimaryImageOrDefault()
    {
        var primary = Images.FirstOrDefault(x => x.IsPrimary) ?? Images.FirstOrDefault();

        return primary?.Url ?? "/img/placeholders/listing-default.jpg";
    }
}