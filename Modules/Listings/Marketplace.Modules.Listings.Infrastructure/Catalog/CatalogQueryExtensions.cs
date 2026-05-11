using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Catalog;

public static class CatalogQueryExtensions
{
    public static IQueryable<Listing> Published(this IQueryable<Listing> query)
        => query.Where(x => x.Status == ListingStatus.Active);

    public static IQueryable<Listing> WithCatalogIncludes(this IQueryable<Listing> query)
        => query
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.City)
            .Include(x => x.Images);

    public static IQueryable<Listing> ApplySearch(this IQueryable<Listing> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) return query;
        search = search.Trim();
        return query.Where(x =>
            x.Title.Contains(search) ||
            (x.ShortDescription != null && x.ShortDescription.Contains(search)) ||
            (x.Description != null && x.Description.Contains(search)));
    }

    public static IQueryable<Listing> ApplyCityId(this IQueryable<Listing> query, int? cityId)
    {
        if (!cityId.HasValue) return query;
        return query.Where(x => x.CityId == cityId.Value);
    }

    public static IQueryable<Listing> ApplyCitySlug(this IQueryable<Listing> query, string? citySlug)
    {
        if (string.IsNullOrWhiteSpace(citySlug)) return query;
        return query.Where(x => x.City != null && x.City.Slug == citySlug);
    }

    public static IQueryable<Listing> ApplyCategorySlug(this IQueryable<Listing> query, string? categorySlug)
    {
        if (string.IsNullOrWhiteSpace(categorySlug)) return query;
        return query.Where(x => x.Category != null && x.Category.Slug == categorySlug);
    }

    public static IQueryable<Listing> ApplySubCategoryId(this IQueryable<Listing> query, int? subCategoryId)
    {
        if (!subCategoryId.HasValue) return query;
        return query.Where(x => x.SubCategoryId == subCategoryId.Value);
    }

    public static IQueryable<Listing> ApplySorting(this IQueryable<Listing> query, string? sort)
        => sort?.Trim().ToLowerInvariant() switch
        {
            "rating" => query.OrderByDescending(x => x.Rating).ThenByDescending(x => x.ReviewsCount),
            "title" => query.OrderBy(x => x.Title),
            _ => query.OrderByDescending(x => x.Rating).ThenByDescending(x => x.ReviewsCount)
        };

    public static IQueryable<Listing> ApplyFilter(this IQueryable<Listing> query, CatalogListingFilter filter)
        => query
            .ApplySearch(filter.Search)
            .ApplyCityId(filter.CityId)
            .ApplyCitySlug(filter.CitySlug)
            .ApplyCategorySlug(filter.CategorySlug)
            .ApplySubCategoryId(filter.SubCategoryId);
}
