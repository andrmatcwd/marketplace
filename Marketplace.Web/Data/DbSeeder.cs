using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class DbSeeder
{
    private readonly ListingsDbContext _db;

    public DbSeeder(ListingsDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.Cities.AnyAsync())
            return;

        var random = new Random(42);

        // ----------------------
        // Cities
        // ----------------------
        var citySeeds = new[]
        {
            new CitySeed("Kyiv", "kyiv", 50.4501, 30.5234),
            new CitySeed("Lviv", "lviv", 49.8397, 24.0297),
            new CitySeed("Odessa", "odessa", 46.4825, 30.7233),
            new CitySeed("Kharkiv", "kharkiv", 49.9935, 36.2304),
            new CitySeed("Dnipro", "dnipro", 48.4647, 35.0462),
            new CitySeed("Vinnytsia", "vinnytsia", 49.2331, 28.4682),
            new CitySeed("Ternopil", "ternopil", 49.5535, 25.5948),
            new CitySeed("Ivano-Frankivsk", "ivano-frankivsk", 48.9226, 24.7111),
            new CitySeed("Rivne", "rivne", 50.6199, 26.2516),
            new CitySeed("Poltava", "poltava", 49.5883, 34.5514),
            new CitySeed("Chernihiv", "chernihiv", 51.4982, 31.2893),
            new CitySeed("Uzhhorod", "uzhhorod", 48.6208, 22.2879)
        };

        var cities = citySeeds
            .Select((x, index) => new City
            {
                Name = x.Name,
                Slug = x.Slug,
                Description = $"Services and specialists in {x.Name}.",
                IsPublished = true,
                SortOrder = index + 1
            })
            .ToList();

        _db.Cities.AddRange(cities);

        // ----------------------
        // Categories
        // ----------------------
        var categories = new List<Category>
        {
            new()
            {
                Name = "Medicine",
                Slug = "medicine",
                Description = "Clinics, doctors, diagnostics, therapy, and healthcare services.",
                IsPublished = true,
                SortOrder = 1
            },
            new()
            {
                Name = "Beauty",
                Slug = "beauty",
                Description = "Salons, cosmetology, barbershops, and wellness services.",
                IsPublished = true,
                SortOrder = 2
            },
            new()
            {
                Name = "Home Services",
                Slug = "home-services",
                Description = "Cleaning, plumbing, repairs, moving, and home maintenance.",
                IsPublished = true,
                SortOrder = 3
            },
            new()
            {
                Name = "Education",
                Slug = "education",
                Description = "Tutors, language schools, courses, and child development services.",
                IsPublished = true,
                SortOrder = 4
            },
            new()
            {
                Name = "Legal",
                Slug = "legal",
                Description = "Lawyers, legal consultations, and business support.",
                IsPublished = true,
                SortOrder = 5
            },
            new()
            {
                Name = "Auto Services",
                Slug = "auto-services",
                Description = "Repair, diagnostics, detailing, tires, and roadside help.",
                IsPublished = true,
                SortOrder = 6
            },
            new()
            {
                Name = "Event Services",
                Slug = "event-services",
                Description = "Photographers, catering, decorators, and event planning.",
                IsPublished = true,
                SortOrder = 7
            },
            new()
            {
                Name = "Fitness",
                Slug = "fitness",
                Description = "Gyms, yoga studios, personal trainers, and wellness programs.",
                IsPublished = true,
                SortOrder = 8
            }
        };

        _db.Categories.AddRange(categories);

        // ----------------------
        // SubCategories
        // ----------------------
        var subCategories = new List<SubCategory>();

        void AddSub(string name, string slug, string categorySlug, int sortOrder, string? description = null)
        {
            var category = categories.First(x => x.Slug == categorySlug);

            subCategories.Add(new SubCategory
            {
                Name = name,
                Slug = slug,
                Description = description ?? $"{name} services.",
                IsPublished = true,
                SortOrder = sortOrder,
                Category = category
            });
        }

        AddSub("Dentistry", "dentistry", "medicine", 1);
        AddSub("Therapy", "therapy", "medicine", 2);
        AddSub("Diagnostics", "diagnostics", "medicine", 3);

        AddSub("Cosmetology", "cosmetology", "beauty", 1);
        AddSub("Hair Salon", "hair-salon", "beauty", 2);
        AddSub("Massage", "massage", "beauty", 3);

        AddSub("Cleaning", "cleaning", "home-services", 1);
        AddSub("Plumbing", "plumbing", "home-services", 2);
        AddSub("Electrical Repair", "electrical-repair", "home-services", 3);

        AddSub("Language Courses", "language-courses", "education", 1);
        AddSub("Tutors", "tutors", "education", 2);
        AddSub("Programming Courses", "programming-courses", "education", 3);

        AddSub("Family Lawyer", "family-lawyer", "legal", 1);
        AddSub("Business Lawyer", "business-lawyer", "legal", 2);
        AddSub("Notary Services", "notary-services", "legal", 3);

        AddSub("Car Repair", "car-repair", "auto-services", 1);
        AddSub("Tire Service", "tire-service", "auto-services", 2);
        AddSub("Car Detailing", "car-detailing", "auto-services", 3);

        AddSub("Photography", "photography", "event-services", 1);
        AddSub("Catering", "catering", "event-services", 2);
        AddSub("Event Decoration", "event-decoration", "event-services", 3);

        AddSub("Gym", "gym", "fitness", 1);
        AddSub("Yoga", "yoga", "fitness", 2);
        AddSub("Personal Trainer", "personal-trainer", "fitness", 3);

        _db.SubCategories.AddRange(subCategories);

        // ----------------------
        // Listings
        // ----------------------
        var brandPrefixes = new[]
        {
            "Pro", "Elite", "Nova", "Prime", "Smart", "Urban", "Grand", "City",
            "Expert", "Modern", "Bright", "Family", "Local", "Premium", "Best", "First"
        };

        var brandSuffixes = new[]
        {
            "Center", "Studio", "Clinic", "Group", "Service", "Lab", "Point", "Hub",
            "Care", "Support", "Works", "Team", "Plus", "Space", "House", "Solutions"
        };

        var streetNames = new[]
        {
            "Shevchenka", "Franka", "Hrushevskoho", "Bandery", "Soborna", "Mazepy",
            "Centralna", "Lychakivska", "Zelena", "Naukova", "Peremohy", "Stepana Bandery",
            "Kyivska", "Dniprovska", "Kharkivska", "Volodymyrska"
        };

        var firstNames = new[]
        {
            "Anna", "Ihor", "Olena", "Maksym", "Andrii", "Sofiia", "Kateryna", "Taras",
            "Oksana", "Yulia", "Roman", "Viktor", "Natalia", "Dmytro", "Alina", "Mykhailo"
        };

        var reviewPhrases = new[]
        {
            "Great service, highly recommend.",
            "Everything was professional and on time.",
            "Very friendly staff and good communication.",
            "Good value for money, would contact again.",
            "The quality exceeded my expectations.",
            "Fast response and clean work.",
            "Comfortable experience and clear pricing.",
            "A reliable service with a strong result."
        };

        var categoryDescriptions = new Dictionary<string, string[]>
        {
            ["medicine"] = new[]
            {
                "Modern clinic with qualified specialists and flexible appointments.",
                "Professional medical support, diagnostics, and patient-focused care.",
                "Comfortable healthcare service with transparent communication and reviews."
            },
            ["beauty"] = new[]
            {
                "Experienced specialists, modern techniques, and comfortable service.",
                "Beauty services focused on quality, hygiene, and visible results.",
                "Professional beauty studio with individual approach and strong reviews."
            },
            ["home-services"] = new[]
            {
                "Reliable local professionals for home maintenance and urgent requests.",
                "Fast service, clear pricing, and convenient scheduling for your home tasks.",
                "Trusted home service team for cleaning, repairs, and household support."
            },
            ["education"] = new[]
            {
                "Structured learning programs for adults and children.",
                "Experienced teachers, practical lessons, and flexible schedules.",
                "Educational services with real progress and student-friendly format."
            },
            ["legal"] = new[]
            {
                "Professional legal support for individuals and businesses.",
                "Clear legal consultations with practical next steps.",
                "Reliable legal assistance with attention to detail and deadlines."
            },
            ["auto-services"] = new[]
            {
                "Trusted specialists for diagnostics, repair, and car care.",
                "Fast service and transparent estimates for everyday auto needs.",
                "Professional auto solutions with modern tools and experienced staff."
            },
            ["event-services"] = new[]
            {
                "Creative event services for celebrations, business events, and private occasions.",
                "Experienced event team with planning, support, and reliable delivery.",
                "Professional event services with quality execution and clear organization."
            },
            ["fitness"] = new[]
            {
                "Programs for health, movement, and long-term results.",
                "Professional trainers and flexible plans for different goals.",
                "Fitness services focused on progress, comfort, and motivation."
            }
        };

        var listings = new List<Listing>();

        const int listingsCount = 400;

        for (int i = 1; i <= listingsCount; i++)
        {
            var cityIndex = random.Next(cities.Count);
            var city = cities[cityIndex];
            var citySeed = citySeeds[cityIndex];

            var subCategory = subCategories[random.Next(subCategories.Count)];
            var category = subCategory.Category!;

            var prefix = brandPrefixes[random.Next(brandPrefixes.Length)];
            var suffix = brandSuffixes[random.Next(brandSuffixes.Length)];

            var title = $"{prefix} {subCategory.Name} {suffix}";
            if (random.NextDouble() > 0.55)
            {
                title += $" {city.Name}";
            }

            title = MakeUniqueTitle(title, i);

            var slug = GenerateSlug(title);

            var rating = Math.Round(3.2 + random.NextDouble() * 1.8, 1);
            var reviewsCount = random.Next(4, 180);

            var latOffset = (random.NextDouble() - 0.5) * 0.18;
            var lngOffset = (random.NextDouble() - 0.5) * 0.18;

            var created = DateTime.UtcNow.AddDays(-random.Next(0, 500));

            var listing = new Listing
            {
                Title = title,
                Slug = slug,
                ShortDescription = categoryDescriptions[category.Slug][random.Next(categoryDescriptions[category.Slug].Length)],
                Description = BuildListingDescription(title, city.Name, category.Name, subCategory.Name),
                Address = $"{streetNames[random.Next(streetNames.Length)]} St., {random.Next(1, 160)}, {city.Name}",
                Phone = GeneratePhone(random),
                Email = $"info{random.Next(100, 999)}@{GenerateSlug(prefix + suffix)}.ua",
                Website = $"https://{GenerateSlug(prefix + "-" + suffix)}-{random.Next(10, 999)}.ua",
                Rating = rating,
                ReviewsCount = reviewsCount,
                Latitude = Math.Round(citySeed.Latitude + latOffset, 6),
                Longitude = Math.Round(citySeed.Longitude + lngOffset, 6),
                Status = ListingStatus.Active,
                CreatedAtUtc = created,
                UpdatedAtUtc = random.NextDouble() > 0.45
                    ? DateTime.UtcNow.AddDays(-random.Next(0, 120))
                    : created,
                City = city,
                Category = category,
                SubCategory = subCategory
            };

            listing.Images = BuildImages(listing);
            listing.Reviews = BuildReviews(listing, random, firstNames, reviewPhrases);

            listings.Add(listing);
        }

        _db.Listings.AddRange(listings);

        await _db.SaveChangesAsync();
    }

    private static List<Image> BuildImages(Listing listing)
    {
        return new List<Image>
        {
            new()
            {
                Listing = listing,
                Url = "/img/placeholders/listing-default.jpg",
                Alt = listing.Title,
                IsPrimary = true,
                SortOrder = 1
            },
            new()
            {
                Listing = listing,
                Url = "/img/placeholders/listing-default.jpg",
                Alt = $"{listing.Title} gallery 2",
                IsPrimary = false,
                SortOrder = 2
            },
            new()
            {
                Listing = listing,
                Url = "/img/placeholders/listing-default.jpg",
                Alt = $"{listing.Title} gallery 3",
                IsPrimary = false,
                SortOrder = 3
            }
        };
    }

    private static List<Review> BuildReviews(
        Listing listing,
        Random random,
        string[] firstNames,
        string[] reviewPhrases)
    {
        var count = random.Next(2, 10);
        var reviews = new List<Review>();

        for (int i = 0; i < count; i++)
        {
            var created = DateTime.UtcNow.AddDays(-random.Next(0, 180));
            reviews.Add(new Review
            {
                Listing = listing,
                AuthorName = firstNames[random.Next(firstNames.Length)],
                Rating = Math.Round(3.0 + random.NextDouble() * 2.0, 1),
                Text = reviewPhrases[random.Next(reviewPhrases.Length)],
                CreatedAtUtc = created,
                UpdatedAtUtc = created
            });
        }

        return reviews;
    }

    private static string BuildListingDescription(string title, string cityName, string categoryName, string subCategoryName)
    {
        return $"""
<p><strong>{title}</strong> provides {subCategoryName.ToLower()} services in {cityName}. The service is part of the {categoryName.ToLower()} category and is designed for users who want a clear, professional, and convenient experience.</p>
<p>Clients choose this listing because of strong communication, flexible scheduling, and a practical service approach. The page includes contacts, address, ratings, and reviews to help compare available options.</p>
<p>If you are looking for {subCategoryName.ToLower()} in {cityName}, this listing can be a useful option for quick selection and contact.</p>
""";
    }

    private static string MakeUniqueTitle(string title, int index)
    {
        return $"{title} {index}";
    }

    private static string GeneratePhone(Random random)
    {
        return $"+380{random.Next(50, 99)}{random.Next(1000000, 9999999)}";
    }

    private static string GenerateSlug(string text)
    {
        return text
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace(",", "")
            .Replace(".", "")
            .Replace("/", "-")
            .Replace("&", "and")
            .Replace("--", "-");
    }

    private sealed record CitySeed(string Name, string Slug, double Latitude, double Longitude);
}
