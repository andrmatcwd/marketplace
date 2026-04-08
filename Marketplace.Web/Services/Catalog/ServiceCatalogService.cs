using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Services.Catalog;

public sealed class ServiceCatalogService : IServiceCatalogService
{
    private static readonly List<ServiceItemViewModel> Seed = GenerateSeed().ToList();

    private static IReadOnlyList<ServiceItemViewModel> GenerateSeed()
    {
        var categories = new[]
        {
            ("beauty", "Beauty"),
            ("repair", "Repair"),
            ("education", "Education"),
            ("cleaning", "Cleaning")
        };

        var cities = new[] { "Kyiv", "Lviv", "Odesa", "Dnipro", "Kharkiv" };
        var titles = new[]
        {
            "Hair Styling", "Makeup Artist", "Nail Service",
            "Laptop Repair", "Phone Repair", "Bike Repair",
            "Math Tutor", "English Coach", "UI Design Mentoring",
            "Home Cleaning", "Deep Cleaning", "Window Cleaning"
        };

        var images = new[]
        {
            "https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9",
            "https://images.unsplash.com/photo-1517841905240-472988babdf9",
            "https://images.unsplash.com/photo-1581578731548-c64695cc6952",
            "https://images.unsplash.com/photo-1518779578993-ec3579fee39f",
            "https://images.unsplash.com/photo-1584697964403-3d98f06c3b52"
        };

        var random = new Random();
        var list = new List<ServiceItemViewModel>();
        int id = 1;

        for (int i = 0; i < 40; i++)
        {
            var category = categories[random.Next(categories.Length)];
            var title = titles[random.Next(titles.Length)];

            list.Add(new ServiceItemViewModel
            {
                Id = id++,
                Title = title,
                Description = $"{title} professional service with high quality and experience.",
                Category = category.Item1,
                Price = random.Next(20, 150),
                Currency = "USD",
                City = cities[random.Next(cities.Length)],
                Rating = Math.Round(4.0 + random.NextDouble(), 1),
                IsOnline = random.Next(0, 2) == 1,
                IsOffline = true,
                ImageUrl = images[random.Next(images.Length)],
                ImageUrls = new List<string>
                {
                    "https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9",
                    "https://images.unsplash.com/photo-1517841905240-472988babdf9",
                    "https://images.unsplash.com/photo-1581578731548-c64695cc6952"
                },
                Latitude = 50.4501,
                Longitude = 30.5234
            });
        }

        return list;
    }

    public Task<IReadOnlyList<ServiceCategoryViewModel>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
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
            query = query.Where(x => request.Categories.Contains(x.Category));

        if (request.PriceFrom.HasValue)
            query = query.Where(x => x.Price >= request.PriceFrom.Value);

        if (request.PriceTo.HasValue)
            query = query.Where(x => x.Price <= request.PriceTo.Value);

        if (request.OnlineOnly)
            query = query.Where(x => x.IsOnline);

        if (request.OfflineOnly)
            query = query.Where(x => x.IsOffline);

        if (request.RatingFrom.HasValue)
            query = query.Where(x => x.Rating >= request.RatingFrom.Value);

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

    public Task<ServiceItemViewModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = Seed.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(item);
    }

    public Task CreateAsync(ServiceItemViewModel model, CancellationToken cancellationToken = default)
    {
        model.Id = Seed.Count == 0 ? 1 : Seed.Max(x => x.Id) + 1;

        if (string.IsNullOrWhiteSpace(model.ImageUrl))
            model.ImageUrl = "https://via.placeholder.com/600x400?text=Service";

        model.ImageUrls ??= new List<string>();

        Seed.Add(model);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ServiceItemViewModel model, CancellationToken cancellationToken = default)
    {
        var existing = Seed.FirstOrDefault(x => x.Id == model.Id);
        if (existing is null)
            return Task.CompletedTask;

        existing.Title = model.Title;
        existing.Description = model.Description;
        existing.Category = model.Category;
        existing.Price = model.Price;
        existing.Currency = model.Currency;
        existing.City = model.City;
        existing.AddressLine = model.AddressLine;
        existing.Latitude = model.Latitude;
        existing.Longitude = model.Longitude;
        existing.Rating = model.Rating;
        existing.IsOnline = model.IsOnline;
        existing.IsOffline = model.IsOffline;
        existing.ImageUrl = model.ImageUrl;
        existing.ImageUrls = model.ImageUrls ?? new List<string>();

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = Seed.FirstOrDefault(x => x.Id == id);
        if (existing is not null)
            Seed.Remove(existing);

        return Task.CompletedTask;
    }
}