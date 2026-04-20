namespace Marketplace.Web.Models.Listings;

public sealed class RelatedListingVm
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }
    public string? CityName { get; set; }
    public string? SubCategoryName { get; set; }

    public double Rating { get; set; }

    public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);
    public bool HasRating => Rating > 0;
    public string RatingFormatted => Rating > 0 ? Rating.ToString("0.0") : string.Empty;
}