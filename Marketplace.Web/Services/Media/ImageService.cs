namespace Marketplace.Web.Services.Media;

public sealed class ImageService : IImageService
{
    public string GetListingPlaceholder() => "/img/placeholders/listing-default.jpg";

    public string GetCategoryPlaceholder() => "/img/placeholders/category-default.jpg";

    public string GetCityPlaceholder() => "/img/placeholders/city-default.jpg";
}