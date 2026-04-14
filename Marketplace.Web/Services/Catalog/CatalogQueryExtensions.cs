using Marketplace.Web.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Catalog;

public static class CatalogQueryExtensions
{
    public static IQueryable<Listing> Published(this IQueryable<Listing> query)
    {
        return query.Where(x => x.IsPublished);
    }

    public static IQueryable<Listing> WithCatalogIncludes(this IQueryable<Listing> query)
    {
        return query
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.City)
            .Include(x => x.Images);
    }

    public static IQueryable<Listing> ApplySearch(this IQueryable<Listing> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        search = search.Trim();

        return query.Where(x =>
            x.Title.Contains(search) ||
            (x.ShortDescription != null && x.ShortDescription.Contains(search)) ||
            (x.Description != null && x.Description.Contains(search)));
    }

    public static IQueryable<Listing> ApplyCity(this IQueryable<Listing> query, string? citySlug)
    {
        if (string.IsNullOrWhiteSpace(citySlug))
        {
            return query;
        }

        return query.Where(x => x.City != null && x.City.Slug == citySlug);
    }

    public static IQueryable<Listing> ApplyCategory(this IQueryable<Listing> query, string? categorySlug)
    {
        if (string.IsNullOrWhiteSpace(categorySlug))
        {
            return query;
        }

        return query.Where(x => x.Category != null && x.Category.Slug == categorySlug);
    }

    public static IQueryable<Listing> ApplySubCategory(this IQueryable<Listing> query, string? subCategorySlug)
    {
        if (string.IsNullOrWhiteSpace(subCategorySlug))
        {
            return query;
        }

        return query.Where(x => x.SubCategory != null && x.SubCategory.Slug == subCategorySlug);
    }

    public static IQueryable<Listing> ApplySorting(this IQueryable<Listing> query, string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            //"newest" => query.OrderByDescending(x => x.CreatedAtUtc),
            "rating" => query.OrderByDescending(x => x.Rating).ThenByDescending(x => x.ReviewsCount),
            "title" => query.OrderBy(x => x.Title),
            _ => query.OrderByDescending(x => x.Rating).ThenByDescending(x => x.ReviewsCount)
        };
    }
}