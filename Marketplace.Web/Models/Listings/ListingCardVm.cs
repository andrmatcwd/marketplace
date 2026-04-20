namespace Marketplace.Web.Models.Listings;

public sealed class ListingCardVm
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }
    public string? ImageAlt { get; set; }

    public string? ShortDescription { get; set; }

    public string? CategoryName { get; set; }
    public string? CategoryUrl { get; set; }

    public string? SubCategoryName { get; set; }
    public string? SubCategoryUrl { get; set; }

    public string? CityName { get; set; }
    public string? CityUrl { get; set; }

    public double Rating { get; set; }
    public int ReviewsCount { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsVerified { get; set; }

    public bool HasRating => Rating > 0;
    public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);
    public bool HasDescription => !string.IsNullOrWhiteSpace(ShortDescription);

    public string RatingFormatted => Rating > 0 ? Rating.ToString("0.0") : string.Empty;

    public string ReviewsFormatted => ReviewsCount > 0 ? $"({ReviewsCount})" : string.Empty;

    public string GetImageOrDefault()
    {
        return string.IsNullOrWhiteSpace(ImageUrl)
            ? "/img/placeholders/listing-default.jpg"
            : ImageUrl!;
    }
}