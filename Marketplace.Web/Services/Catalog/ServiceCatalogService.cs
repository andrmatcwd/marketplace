using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Services.Catalog;

public sealed class ServiceCatalogService : IServiceCatalogService
{
    private static readonly IReadOnlyList<ServiceItemViewModel> Seed =
    [
        new() { Id = 1, Title = "Hair Styling", Description = "Professional hair styling for events and daily looks.", Category = "beauty", Price = 50, City = "Kyiv", Rating = 4.8, IsOnline = false, IsOffline = true },
        new() { Id = 2, Title = "Math Tutor", Description = "Online math tutoring for school students.", Category = "education", Price = 25, City = "Lviv", Rating = 4.9, IsOnline = true, IsOffline = false },
        new() { Id = 3, Title = "Home Cleaning", Description = "Full apartment cleaning service.", Category = "cleaning", Price = 80, City = "Kyiv", Rating = 4.7, IsOnline = false, IsOffline = true },
        new() { Id = 4, Title = "Laptop Repair", Description = "Hardware diagnostics and repair.", Category = "repair", Price = 65, City = "Odesa", Rating = 4.6, IsOnline = false, IsOffline = true },
        new() { Id = 5, Title = "English Speaking Coach", Description = "One-on-one speaking practice sessions.", Category = "education", Price = 30, City = "Dnipro", Rating = 5.0, IsOnline = true, IsOffline = true },
        new() { Id = 6, Title = "Makeup Artist", Description = "Event and bridal makeup services.", Category = "beauty", Price = 95, City = "Kyiv", Rating = 4.9, IsOnline = false, IsOffline = true },
        new() { Id = 7, Title = "Window Cleaning", Description = "Residential and office window cleaning.", Category = "cleaning", Price = 40, City = "Kharkiv", Rating = 4.5, IsOnline = false, IsOffline = true },
        new() { Id = 8, Title = "Phone Repair", Description = "Screen and battery replacement.", Category = "repair", Price = 55, City = "Kyiv", Rating = 4.8, IsOnline = false, IsOffline = true },
        new() { Id = 9, Title = "UI Design Mentoring", Description = "Review and coaching sessions for designers.", Category = "education", Price = 45, City = "Remote", Rating = 4.9, IsOnline = true, IsOffline = false },
        new() { Id = 10, Title = "Nail Service", Description = "Classic and gel manicure.", Category = "beauty", Price = 35, City = "Lviv", Rating = 4.7, IsOnline = false, IsOffline = true },
        new() { Id = 11, Title = "Deep Cleaning", Description = "Detailed kitchen and bathroom cleaning.", Category = "cleaning", Price = 120, City = "Kyiv", Rating = 4.8, IsOnline = false, IsOffline = true },
        new() { Id = 12, Title = "Bike Repair", Description = "Maintenance and tune-up for city bikes.", Category = "repair", Price = 28, City = "Lviv", Rating = 4.6, IsOnline = false, IsOffline = true }
    ];

    public Task<IReadOnlyList<ServiceCategoryViewModel>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ServiceCategoryViewModel> categories =
        [
            new() { Value = "beauty", Label = "Beauty" },
            new() { Value = "repair", Label = "Repair" },
            new() { Value = "education", Label = "Education" },
            new() { Value = "cleaning", Label = "Cleaning" }
        ];

        return Task.FromResult(categories);
    }

    public Task<PagedResult<ServiceItemViewModel>> GetServicesAsync(
        ServicesFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ServiceItemViewModel> query = Seed.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x =>
                x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.City.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (request.Categories.Count > 0)
        {
            query = query.Where(x => request.Categories.Contains(x.Category));
        }

        if (request.PriceFrom.HasValue)
        {
            query = query.Where(x => x.Price >= request.PriceFrom.Value);
        }

        if (request.PriceTo.HasValue)
        {
            query = query.Where(x => x.Price <= request.PriceTo.Value);
        }

        if (request.OnlineOnly)
        {
            query = query.Where(x => x.IsOnline);
        }

        if (request.OfflineOnly)
        {
            query = query.Where(x => x.IsOffline);
        }

        if (request.RatingFrom.HasValue)
        {
            query = query.Where(x => x.Rating >= request.RatingFrom.Value);
        }

        query = request.SortBy.ToLowerInvariant() switch
        {
            "price_asc" => query.OrderBy(x => x.Price),
            "price_desc" => query.OrderByDescending(x => x.Price),
            "rating_desc" => query.OrderByDescending(x => x.Rating),
            "title_asc" => query.OrderBy(x => x.Title),
            _ => query.OrderByDescending(x => x.Id)
        };

        var totalItems = query.Count();
        var pageSize = request.PageSize <= 0 ? 9 : request.PageSize;
        var page = request.Page <= 0 ? 1 : request.Page;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<ServiceItemViewModel>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        });
    }
}