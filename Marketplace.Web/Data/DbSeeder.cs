using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class DbSeeder
{
    private static readonly (string Name, string Slug, double Lat, double Lng)[] CitySeeds =
    [
        ("Kyiv",            "kyiv",            50.4501, 30.5234),
        ("Lviv",            "lviv",            49.8397, 24.0297),
        ("Odessa",          "odessa",          46.4825, 30.7233),
        ("Kharkiv",         "kharkiv",         49.9935, 36.2304),
        ("Dnipro",          "dnipro",          48.4647, 35.0462),
        ("Vinnytsia",       "vinnytsia",       49.2331, 28.4682),
        ("Ternopil",        "ternopil",        49.5535, 25.5948),
        ("Ivano-Frankivsk", "ivano-frankivsk", 48.9226, 24.7111),
        ("Rivne",           "rivne",           50.6199, 26.2516),
        ("Poltava",         "poltava",         49.5883, 34.5514),
        ("Chernihiv",       "chernihiv",       51.4982, 31.2893),
        ("Uzhhorod",        "uzhhorod",        48.6208, 22.2879)
    ];

    private readonly ListingsDbContext _db;

    public DbSeeder(ListingsDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (!await _db.Cities.AnyAsync())
        {
            var random = new Random(42);

            var cities = SeedCities();
            var (categories, subCategories) = SeedCategoriesAndSubCategories();

            _db.Cities.AddRange(cities);
            _db.Categories.AddRange(categories);
            _db.SubCategories.AddRange(subCategories);

            var listings = BuildListings(cities, subCategories, random);
            _db.Listings.AddRange(listings);

            await _db.SaveChangesAsync();
        }

        await SeedSubscriptionsAsync();
    }

    private async Task SeedSubscriptionsAsync()
    {
        if (await _db.SubscriptionPlans.AnyAsync())
            return;

        var random = new Random(7);

        var plans = SubscriptionSeedData.BuildPlans();
        _db.SubscriptionPlans.AddRange(plans);
        await _db.SaveChangesAsync();

        var listings = await _db.Listings
            .OrderBy(x => x.Id)
            .Take(60)
            .ToListAsync();

        if (listings.Count < 10)
            return;

        var subscriptions = SubscriptionSeedData.BuildSubscriptions(listings, plans, random);
        _db.ListingSubscriptions.AddRange(subscriptions);
        await _db.SaveChangesAsync();
    }

    private static List<City> SeedCities()
        => CitySeeds
            .Select((x, i) => new City
            {
                Name = x.Name,
                Slug = x.Slug,
                Description = $"Services and specialists in {x.Name}.",
                IsPublished = true,
                SortOrder = i + 1
            })
            .ToList();

    private static (List<Category> Categories, List<SubCategory> SubCategories) SeedCategoriesAndSubCategories()
    {
        var categories = new List<Category>
        {
            new() { Name = "Medicine",       Slug = "medicine",       Description = "Clinics, doctors, diagnostics, therapy, and healthcare services.",           IsPublished = true, SortOrder = 1 },
            new() { Name = "Beauty",         Slug = "beauty",         Description = "Salons, cosmetology, barbershops, and wellness services.",                   IsPublished = true, SortOrder = 2 },
            new() { Name = "Home Services",  Slug = "home-services",  Description = "Cleaning, plumbing, repairs, moving, and home maintenance.",                 IsPublished = true, SortOrder = 3 },
            new() { Name = "Education",      Slug = "education",      Description = "Tutors, language schools, courses, and child development services.",         IsPublished = true, SortOrder = 4 },
            new() { Name = "Legal",          Slug = "legal",          Description = "Lawyers, legal consultations, and business support.",                        IsPublished = true, SortOrder = 5 },
            new() { Name = "Auto Services",  Slug = "auto-services",  Description = "Repair, diagnostics, detailing, tires, and roadside help.",                 IsPublished = true, SortOrder = 6 },
            new() { Name = "Event Services", Slug = "event-services", Description = "Photographers, catering, decorators, and event planning.",                   IsPublished = true, SortOrder = 7 },
            new() { Name = "Fitness",        Slug = "fitness",        Description = "Gyms, yoga studios, personal trainers, and wellness programs.",              IsPublished = true, SortOrder = 8 },
        };

        Category Cat(string slug) => categories.First(x => x.Slug == slug);

        var subCategories = new List<SubCategory>
        {
            new() { Name = "Dentistry",           Slug = "dentistry",           Description = "Dentistry services.",           IsPublished = true, SortOrder = 1, Category = Cat("medicine") },
            new() { Name = "Therapy",             Slug = "therapy",             Description = "Therapy services.",             IsPublished = true, SortOrder = 2, Category = Cat("medicine") },
            new() { Name = "Diagnostics",         Slug = "diagnostics",         Description = "Diagnostics services.",         IsPublished = true, SortOrder = 3, Category = Cat("medicine") },

            new() { Name = "Cosmetology",         Slug = "cosmetology",         Description = "Cosmetology services.",         IsPublished = true, SortOrder = 1, Category = Cat("beauty") },
            new() { Name = "Hair Salon",          Slug = "hair-salon",          Description = "Hair Salon services.",          IsPublished = true, SortOrder = 2, Category = Cat("beauty") },
            new() { Name = "Massage",             Slug = "massage",             Description = "Massage services.",             IsPublished = true, SortOrder = 3, Category = Cat("beauty") },

            new() { Name = "Cleaning",            Slug = "cleaning",            Description = "Cleaning services.",            IsPublished = true, SortOrder = 1, Category = Cat("home-services") },
            new() { Name = "Plumbing",            Slug = "plumbing",            Description = "Plumbing services.",            IsPublished = true, SortOrder = 2, Category = Cat("home-services") },
            new() { Name = "Electrical Repair",   Slug = "electrical-repair",   Description = "Electrical Repair services.",   IsPublished = true, SortOrder = 3, Category = Cat("home-services") },

            new() { Name = "Language Courses",    Slug = "language-courses",    Description = "Language Courses services.",    IsPublished = true, SortOrder = 1, Category = Cat("education") },
            new() { Name = "Tutors",              Slug = "tutors",              Description = "Tutors services.",              IsPublished = true, SortOrder = 2, Category = Cat("education") },
            new() { Name = "Programming Courses", Slug = "programming-courses", Description = "Programming Courses services.", IsPublished = true, SortOrder = 3, Category = Cat("education") },

            new() { Name = "Family Lawyer",       Slug = "family-lawyer",       Description = "Family Lawyer services.",       IsPublished = true, SortOrder = 1, Category = Cat("legal") },
            new() { Name = "Business Lawyer",     Slug = "business-lawyer",     Description = "Business Lawyer services.",     IsPublished = true, SortOrder = 2, Category = Cat("legal") },
            new() { Name = "Notary Services",     Slug = "notary-services",     Description = "Notary Services services.",     IsPublished = true, SortOrder = 3, Category = Cat("legal") },

            new() { Name = "Car Repair",          Slug = "car-repair",          Description = "Car Repair services.",          IsPublished = true, SortOrder = 1, Category = Cat("auto-services") },
            new() { Name = "Tire Service",        Slug = "tire-service",        Description = "Tire Service services.",        IsPublished = true, SortOrder = 2, Category = Cat("auto-services") },
            new() { Name = "Car Detailing",       Slug = "car-detailing",       Description = "Car Detailing services.",       IsPublished = true, SortOrder = 3, Category = Cat("auto-services") },

            new() { Name = "Photography",         Slug = "photography",         Description = "Photography services.",         IsPublished = true, SortOrder = 1, Category = Cat("event-services") },
            new() { Name = "Catering",            Slug = "catering",            Description = "Catering services.",            IsPublished = true, SortOrder = 2, Category = Cat("event-services") },
            new() { Name = "Event Decoration",    Slug = "event-decoration",    Description = "Event Decoration services.",    IsPublished = true, SortOrder = 3, Category = Cat("event-services") },

            new() { Name = "Gym",                 Slug = "gym",                 Description = "Gym services.",                IsPublished = true, SortOrder = 1, Category = Cat("fitness") },
            new() { Name = "Yoga",                Slug = "yoga",                Description = "Yoga services.",               IsPublished = true, SortOrder = 2, Category = Cat("fitness") },
            new() { Name = "Personal Trainer",    Slug = "personal-trainer",    Description = "Personal Trainer services.",   IsPublished = true, SortOrder = 3, Category = Cat("fitness") },
        };

        return (categories, subCategories);
    }

    private static List<Listing> BuildListings(
        List<City> cities,
        List<SubCategory> subCategories,
        Random random)
    {
        var cityCoords = CitySeeds.ToDictionary(x => x.Slug, x => (x.Lat, x.Lng));

        var listings = new List<Listing>(400);

        for (int i = 1; i <= 400; i++)
        {
            var city = cities[random.Next(cities.Count)];
            var subCategory = subCategories[random.Next(subCategories.Count)];
            var category = subCategory.Category;

            var prefix = ListingBuilder.BrandPrefixes[random.Next(ListingBuilder.BrandPrefixes.Length)];
            var suffix = ListingBuilder.BrandSuffixes[random.Next(ListingBuilder.BrandSuffixes.Length)];

            var title = $"{prefix} {subCategory.Name} {suffix}";
            if (random.NextDouble() > 0.55)
                title += $" {city.Name}";

            title = ListingBuilder.MakeUniqueTitle(title, i);
            var slug = ListingBuilder.GenerateSlug(title);

            var (baseLat, baseLng) = cityCoords[city.Slug];
            var created = DateTime.UtcNow.AddDays(-random.Next(0, 500));

            var descriptions = ListingBuilder.CategoryDescriptions[category.Slug];

            var listing = new Listing
            {
                Title = title,
                Slug = slug,
                ShortDescription = descriptions[random.Next(descriptions.Length)],
                Description = ListingBuilder.BuildDescription(title, city.Name, category.Name, subCategory.Name),
                Address = $"{ListingBuilder.StreetNames[random.Next(ListingBuilder.StreetNames.Length)]} St., {random.Next(1, 160)}, {city.Name}",
                Phone = ListingBuilder.GeneratePhone(random),
                Email = $"info{random.Next(100, 999)}@{ListingBuilder.GenerateSlug(prefix + suffix)}.ua",
                Website = $"https://{ListingBuilder.GenerateSlug(prefix + "-" + suffix)}-{random.Next(10, 999)}.ua",
                Rating = Math.Round(3.2 + random.NextDouble() * 1.8, 1),
                ReviewsCount = random.Next(4, 180),
                Latitude = Math.Round(baseLat + (random.NextDouble() - 0.5) * 0.18, 6),
                Longitude = Math.Round(baseLng + (random.NextDouble() - 0.5) * 0.18, 6),
                Status = ListingStatus.Active,
                CreatedAtUtc = created,
                UpdatedAtUtc = random.NextDouble() > 0.45
                    ? DateTime.UtcNow.AddDays(-random.Next(0, 120))
                    : created,
                City = city,
                Category = category,
                SubCategory = subCategory
            };

            listing.Images = ListingBuilder.BuildImages(listing);
            listing.Reviews = ListingBuilder.BuildReviews(listing, random);
            listing.Rental = RentalSeedData.Build(listing, category.Slug, random);
            listing.Vacancies = VacancySeedData.Build(category.Slug, city.Name, random);

            listings.Add(listing);
        }

        return listings;
    }
}
