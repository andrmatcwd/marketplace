using Marketplace.Web.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class DbSeeder
{
    private readonly ApplicationDbContext _db;

    public DbSeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.Cities.AnyAsync())
            return;

        // ----------------------
        // Cities
        // ----------------------
        var kyiv = new City { Id = Guid.NewGuid(), Name = "Kyiv", Slug = "kyiv", IsPublished = true };
        var lviv = new City { Id = Guid.NewGuid(), Name = "Lviv", Slug = "lviv", IsPublished = true };
        var odessa = new City { Id = Guid.NewGuid(), Name = "Odessa", Slug = "odessa", IsPublished = true };
        var kharkiv = new City { Id = Guid.NewGuid(), Name = "Kharkiv", Slug = "kharkiv", IsPublished = true };

        _db.Cities.AddRange(kyiv, lviv, odessa, kharkiv);

        // ----------------------
        // Categories
        // ----------------------
        var medicine = new Category { Id = Guid.NewGuid(), Name = "Medicine", Slug = "medicine", IsPublished = true };
        var beauty = new Category { Id = Guid.NewGuid(), Name = "Beauty", Slug = "beauty", IsPublished = true };
        var home = new Category { Id = Guid.NewGuid(), Name = "Home Services", Slug = "home-services", IsPublished = true };

        _db.Categories.AddRange(medicine, beauty, home);

        // ----------------------
        // SubCategories
        // ----------------------
        var dentistry = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Dentistry",
            Slug = "dentistry",
            Category = medicine,
            IsPublished = true
        };

        var therapy = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Therapy",
            Slug = "therapy",
            Category = medicine,
            IsPublished = true
        };

        var cosmetology = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Cosmetology",
            Slug = "cosmetology",
            Category = beauty,
            IsPublished = true
        };

        var cleaning = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Cleaning",
            Slug = "cleaning",
            Category = home,
            IsPublished = true
        };

        _db.SubCategories.AddRange(dentistry, therapy, cosmetology, cleaning);

        // ----------------------
        // Listings
        // ----------------------
        var random = new Random();

        var cities = new[] { kyiv, lviv, odessa, kharkiv };
        var subCategories = new[] { dentistry, therapy, cosmetology, cleaning };

        var listings = new List<Listing>();

        for (int i = 1; i <= 40; i++)
        {
            var city = cities[random.Next(cities.Length)];
            var sub = subCategories[random.Next(subCategories.Length)];

            var title = $"{sub.Name} Service {i} in {city.Name}";

            var listing = new Listing
            {
                Id = Guid.NewGuid(),
                Title = title,
                Slug = GenerateSlug(title),
                ShortDescription = "Professional service with high quality and great reviews.",
                Description = "<p>This is a detailed description of the service. It is SEO-friendly and contains useful information.</p>",
                Address = $"{city.Name} center",
                Phone = "+380991112233",
                Email = "info@example.com",
                Website = "https://example.com",

                Rating = Math.Round(random.NextDouble() * 2 + 3, 1), // 3.0–5.0
                ReviewsCount = random.Next(5, 120),

                Latitude = 49.8397,
                Longitude = 24.0297,

                IsPublished = true,
                CreatedAtUtc = DateTime.UtcNow.AddDays(-random.Next(0, 200)),

                City = city,
                Category = sub.Category,
                SubCategory = sub
            };

            // Images
            listing.Images = new List<ListingImage>
            {
                new ListingImage
                {
                    Id = Guid.NewGuid(),
                    Url = "/img/placeholders/listing-default.jpg",
                    Alt = listing.Title
                }
            };

            // Reviews
            listing.Reviews = Enumerable.Range(1, random.Next(2, 6))
                .Select(x => new ListingReview
                {
                    Id = Guid.NewGuid(),
                    AuthorName = $"User {x}",
                    Rating = Math.Round(random.NextDouble() * 2 + 3, 1),
                    Text = "Great service, highly recommend!",
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-random.Next(0, 100))
                }).ToList();

            listings.Add(listing);
        }

        _db.Listings.AddRange(listings);

        await _db.SaveChangesAsync();
    }

    private static string GenerateSlug(string text)
    {
        return text
            .ToLower()
            .Replace(" ", "-")
            .Replace(",", "")
            .Replace(".", "")
            .Replace("/", "-");
    }
}