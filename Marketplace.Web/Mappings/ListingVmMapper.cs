using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Listings.Forms;
using Marketplace.Web.Navigation;

namespace Marketplace.Web.Mappings;

public sealed class ListingVmMapper : IListingVmMapper
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public ListingVmMapper(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public ListingDetailsPageVm MapDetails(ListingDetailsDto dto, string culture, IReadOnlyCollection<RelatedListingVm>? relatedListings = null)
    {
        return new ListingDetailsPageVm
        {
            Culture = culture,
            Id = dto.Id,
            Title = dto.Title,
            Slug = dto.Slug,
            Url = _urlBuilder.BuildListingUrl(culture, dto.CitySlug, dto.CategorySlug, dto.SubCategorySlug, dto.Slug, dto.Id),

            ShortDescription = dto.ShortDescription,
            Description = dto.Description,

            CategoryName = dto.CategoryName,
            CategorySlug = dto.CategorySlug,
            CategoryUrl = _urlBuilder.BuildCategoryUrl(culture, dto.CitySlug, dto.CategorySlug),

            SubCategoryName = dto.SubCategoryName,
            SubCategorySlug = dto.SubCategorySlug,
            SubCategoryUrl = _urlBuilder.BuildSubCategoryUrl(culture, dto.CitySlug, dto.CategorySlug, dto.SubCategorySlug),

            CityName = dto.CityName,
            CitySlug = dto.CitySlug,
            CityUrl = _urlBuilder.BuildCityUrl(culture, dto.CitySlug),

            Address = dto.Address,
            Rating = dto.Rating,
            ReviewsCount = dto.ReviewsCount,

            Latitude = dto.Latitude,
            Longitude = dto.Longitude,

            Contact = new ListingContactVm
            {
                ContactName = dto.Title,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website
            },

            Gallery = new ListingGalleryVm
            {
                Images = dto.Images
                    .Select(x => new ListingImageVm
                    {
                        Url = x.Url,
                        Alt = string.IsNullOrWhiteSpace(x.Alt) ? dto.Title : x.Alt!,
                        IsPrimary = x.IsPrimary,
                        SortOrder = x.SortOrder
                    })
                    .ToList()
            },

            Reviews = dto.Reviews
                .Select(x => new ListingReviewVm
                {
                    AuthorName = x.AuthorName,
                    Text = x.Text,
                    Rating = x.Rating,
                    CreatedAtUtc = new DateTimeOffset(x.CreatedAtUtc, TimeSpan.Zero)
                })
                .ToList(),

            RelatedListings = relatedListings ?? Array.Empty<RelatedListingVm>(),
            ReviewForm = new CreateListingReviewVm { ListingId = dto.Id },

            ServiceFeatures = BuildServiceFeatures(dto),

            Rental = dto.Rental is null ? null : new RentalDetailsVm
            {
                Price = dto.Rental.Price,
                Rooms = dto.Rental.Rooms,
                Area = dto.Rental.Area,
                Floor = dto.Rental.Floor,
                Features = dto.Rental.Features,
                RoomOptions = dto.Rental.RoomOptions
                    .Select(r => new RentalRoomVm
                    {
                        Title = r.Title,
                        Description = r.Description,
                        Price = r.Price,
                        Area = r.Area,
                        Guests = r.Guests,
                        Beds = r.Beds,
                        ImageUrls = r.ImageUrls,
                        Amenities = r.Amenities
                    })
                    .ToList()
            },

            Vacancies = dto.Vacancies
                .Select(v => new ListingVacancyVm
                {
                    Title = v.Title,
                    Description = v.Description,
                    EmploymentType = v.EmploymentType,
                    SalaryText = v.SalaryText,
                    LocationText = v.LocationText
                })
                .ToList(),
        };
    }

    public RelatedListingVm MapRelatedListing(ListingCardDto dto, string culture)
    {
        return new RelatedListingVm
        {
            Id = dto.Id,
            Title = dto.Title,
            Url = _urlBuilder.BuildListingUrl(culture, dto.CitySlug, dto.CategorySlug, dto.SubCategorySlug, dto.Slug, dto.Id),
            ImageUrl = dto.PrimaryImageUrl,
            CityName = dto.CityName,
            SubCategoryName = dto.SubCategoryName,
            Rating = dto.Rating
        };
    }

    private static IReadOnlyCollection<string> BuildServiceFeatures(ListingDetailsDto dto)
    {
        var features = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.Phone))
            features.Add("Є телефон для звʼязку");

        if (!string.IsNullOrWhiteSpace(dto.Email))
            features.Add("Є email для звернення");

        if (!string.IsNullOrWhiteSpace(dto.Website))
            features.Add("Є сайт або сторінка компанії");

        if (!string.IsNullOrWhiteSpace(dto.Address))
            features.Add("Є фізична адреса");

        if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            features.Add("Є точка на карті");

        if (dto.Images.Count > 0)
            features.Add("Доступна галерея фото");

        if (dto.Rating >= 4.5)
            features.Add("Високий рейтинг");

        if (dto.ReviewsCount >= 10)
            features.Add("Достатньо відгуків для оцінки");

        return features;
    }
}
