using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Utils;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class DbSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public DbSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _dbContext.Cities.AnyAsync(cancellationToken))
        {
            return;
        }

        var kyiv = new City
        {
            Id = Guid.NewGuid(),
            Name = "Kyiv",
            Slug = SlugHelper.Generate("Kyiv"),
            Description = "Capital city"
        };

        var lviv = new City
        {
            Id = Guid.NewGuid(),
            Name = "Lviv",
            Slug = SlugHelper.Generate("Lviv"),
            Description = "Cultural city"
        };

        var repair = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Repair",
            Slug = SlugHelper.Generate("Repair"),
            Description = "Repair and renovation services"
        };

        var medicine = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Medicine",
            Slug = SlugHelper.Generate("Medicine"),
            Description = "Healthcare and clinics"
        };

        var finishing = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Finishing works",
            Slug = SlugHelper.Generate("Finishing works"),
            Description = "Interior finishing services",
            CategoryId = repair.Id
        };

        var dentistry = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = "Dentistry",
            Slug = SlugHelper.Generate("Dentistry"),
            Description = "Dental services",
            CategoryId = medicine.Id
        };

        var repairListing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = "Apartment renovation specialist",
            Slug = SlugHelper.Generate("Apartment renovation specialist"),
            ShortDescription = "Turnkey apartment renovation in Kyiv.",
            Description = "<p>Complete apartment renovation services for homes and offices.</p>",
            Address = "Kyiv, Ukraine",
            Phone = "+380000000000",
            Email = "hello@example.com",
            Website = "https://example.com",
            Rating = 4.8,
            ReviewsCount = 12,
            CategoryId = repair.Id,
            SubCategoryId = finishing.Id,
            CityId = kyiv.Id,
            IsPublished = true
        };

        var dentalListing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = "Private dental clinic",
            Slug = SlugHelper.Generate("Private dental clinic"),
            ShortDescription = "Modern dental treatment in Lviv.",
            Description = "<p>Modern diagnostics and treatment for adults and children.</p>",
            Address = "Lviv, Ukraine",
            Phone = "+380111111111",
            Email = "clinic@example.com",
            Website = "https://clinic.example.com",
            Rating = 4.9,
            ReviewsCount = 18,
            CategoryId = medicine.Id,
            SubCategoryId = dentistry.Id,
            CityId = lviv.Id,
            IsPublished = true
        };

        var repairImage = new ListingImage
        {
            Id = Guid.NewGuid(),
            ListingId = repairListing.Id,
            Url = "/img/placeholders/listing-default.jpg",
            Alt = repairListing.Title,
            IsPrimary = true,
            SortOrder = 0
        };

        var dentalImage = new ListingImage
        {
            Id = Guid.NewGuid(),
            ListingId = dentalListing.Id,
            Url = "/img/placeholders/listing-default.jpg",
            Alt = dentalListing.Title,
            IsPrimary = true,
            SortOrder = 0
        };

        var review1 = new ListingReview
        {
            Id = Guid.NewGuid(),
            ListingId = repairListing.Id,
            AuthorName = "Olena",
            Text = "Great quality and very clear communication.",
            Rating = 5.0
        };

        var review2 = new ListingReview
        {
            Id = Guid.NewGuid(),
            ListingId = dentalListing.Id,
            AuthorName = "Andrii",
            Text = "Very professional doctors and modern equipment.",
            Rating = 4.9
        };

        _dbContext.Cities.AddRange(kyiv, lviv);
        _dbContext.Categories.AddRange(repair, medicine);
        _dbContext.SubCategories.AddRange(finishing, dentistry);
        _dbContext.Listings.AddRange(repairListing, dentalListing);
        _dbContext.ListingImages.AddRange(repairImage, dentalImage);
        _dbContext.ListingReviews.AddRange(review1, review2);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}