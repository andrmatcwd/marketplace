using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

internal static class CatalogDtoMapper
{
    internal static CatalogCityDto ToDto(City city, int listingsCount) => new(
        city.Id, city.Name, city.Slug, city.Description, city.SortOrder, listingsCount);

    internal static CatalogCategoryDto ToDto(Category category, int listingsCount) => new(
        category.Id, category.Name, category.Slug, category.Description, category.Icon,
        category.SortOrder, listingsCount);

    internal static CatalogSubCategoryDto ToDto(SubCategory sub, int listingsCount) => new(
        sub.Id, sub.Name, sub.Slug, sub.Description, sub.Icon, sub.SortOrder,
        sub.CategoryId, sub.Category.Name, sub.Category.Slug, listingsCount);

    internal static ListingCardDto ToCardDto(Listing listing)
    {
        var primary = listing.Images.OrderBy(x => x.SortOrder).FirstOrDefault(x => x.IsPrimary)
                      ?? listing.Images.OrderBy(x => x.SortOrder).FirstOrDefault();

        return new ListingCardDto(
            listing.Id, listing.Title, listing.Slug, listing.ShortDescription,
            listing.Rating, listing.ReviewsCount,
            listing.City.Name, listing.City.Slug,
            listing.Category.Name, listing.Category.Slug,
            listing.SubCategory.Name, listing.SubCategory.Slug,
            primary?.Url, primary?.Alt ?? listing.Title);
    }

    internal static ListingDetailsDto ToDetailsDto(Listing listing) => new(
        listing.Id, listing.Title, listing.Slug,
        listing.ShortDescription, listing.Description,
        listing.Address, listing.Phone, listing.Email, listing.Website,
        listing.Latitude, listing.Longitude,
        listing.Rating, listing.ReviewsCount,
        listing.CityId, listing.City.Name, listing.City.Slug,
        listing.CategoryId, listing.Category.Name, listing.Category.Slug,
        listing.SubCategoryId, listing.SubCategory.Name, listing.SubCategory.Slug,
        listing.Images.OrderBy(x => x.SortOrder)
            .Select(x => new ListingImageDto(x.Url, x.Alt, x.IsPrimary, x.SortOrder))
            .ToList(),
        listing.Reviews.OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new ListingReviewDto(
                x.Reviewer?.UserId ?? x.AuthorName ?? "Anonymous",
                x.Text, x.Rating, x.CreatedAtUtc))
            .ToList(),
        listing.Rental is null ? null : new ListingRentalDto(
            listing.Rental.Price,
            listing.Rental.Rooms,
            listing.Rental.Area,
            listing.Rental.Floor,
            listing.Rental.Features,
            listing.Rental.RoomOptions
                .Select(r => new ListingRentalRoomDto(
                    r.Title, r.Description, r.Price, r.Area, r.Guests, r.Beds,
                    r.ImageUrls, r.Amenities))
                .ToList()),
        listing.Vacancies
            .Select(v => new ListingVacancyDto(
                v.Title, v.Description, v.EmploymentType, v.SalaryText, v.LocationText))
            .ToList());
}
