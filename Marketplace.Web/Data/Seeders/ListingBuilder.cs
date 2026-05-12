using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Web.Data.Seeders;

internal static class ListingBuilder
{
    internal static readonly string[] BrandPrefixes =
    [
        "Pro", "Elite", "Nova", "Prime", "Smart", "Urban", "Grand", "City",
        "Expert", "Modern", "Bright", "Family", "Local", "Premium", "Best", "First"
    ];

    internal static readonly string[] BrandSuffixes =
    [
        "Center", "Studio", "Clinic", "Group", "Service", "Lab", "Point", "Hub",
        "Care", "Support", "Works", "Team", "Plus", "Space", "House", "Solutions"
    ];

    internal static readonly string[] StreetNames =
    [
        "Shevchenka", "Franka", "Hrushevskoho", "Bandery", "Soborna", "Mazepy",
        "Centralna", "Lychakivska", "Zelena", "Naukova", "Peremohy", "Stepana Bandery",
        "Kyivska", "Dniprovska", "Kharkivska", "Volodymyrska"
    ];

    internal static readonly string[] FirstNames =
    [
        "Anna", "Ihor", "Olena", "Maksym", "Andrii", "Sofiia", "Kateryna", "Taras",
        "Oksana", "Yulia", "Roman", "Viktor", "Natalia", "Dmytro", "Alina", "Mykhailo"
    ];

    internal static readonly string[] ReviewPhrases =
    [
        "Great service, highly recommend.",
        "Everything was professional and on time.",
        "Very friendly staff and good communication.",
        "Good value for money, would contact again.",
        "The quality exceeded my expectations.",
        "Fast response and clean work.",
        "Comfortable experience and clear pricing.",
        "A reliable service with a strong result."
    ];

    internal static readonly Dictionary<string, string[]> CategoryDescriptions = new()
    {
        ["medicine"] =
        [
            "Modern clinic with qualified specialists and flexible appointments.",
            "Professional medical support, diagnostics, and patient-focused care.",
            "Comfortable healthcare service with transparent communication and reviews."
        ],
        ["beauty"] =
        [
            "Experienced specialists, modern techniques, and comfortable service.",
            "Beauty services focused on quality, hygiene, and visible results.",
            "Professional beauty studio with individual approach and strong reviews."
        ],
        ["home-services"] =
        [
            "Reliable local professionals for home maintenance and urgent requests.",
            "Fast service, clear pricing, and convenient scheduling for your home tasks.",
            "Trusted home service team for cleaning, repairs, and household support."
        ],
        ["education"] =
        [
            "Structured learning programs for adults and children.",
            "Experienced teachers, practical lessons, and flexible schedules.",
            "Educational services with real progress and student-friendly format."
        ],
        ["legal"] =
        [
            "Professional legal support for individuals and businesses.",
            "Clear legal consultations with practical next steps.",
            "Reliable legal assistance with attention to detail and deadlines."
        ],
        ["auto-services"] =
        [
            "Trusted specialists for diagnostics, repair, and car care.",
            "Fast service and transparent estimates for everyday auto needs.",
            "Professional auto solutions with modern tools and experienced staff."
        ],
        ["event-services"] =
        [
            "Creative event services for celebrations, business events, and private occasions.",
            "Experienced event team with planning, support, and reliable delivery.",
            "Professional event services with quality execution and clear organization."
        ],
        ["fitness"] =
        [
            "Programs for health, movement, and long-term results.",
            "Professional trainers and flexible plans for different goals.",
            "Fitness services focused on progress, comfort, and motivation."
        ]
    };

    internal static List<Image> BuildImages(Listing listing) =>
    [
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
    ];

    internal static List<Review> BuildReviews(Listing listing, Random random)
    {
        var count = random.Next(2, 10);
        var reviews = new List<Review>(count);

        for (int i = 0; i < count; i++)
        {
            var created = DateTime.UtcNow.AddDays(-random.Next(0, 180));
            reviews.Add(new Review
            {
                Listing = listing,
                AuthorName = FirstNames[random.Next(FirstNames.Length)],
                Rating = Math.Round(3.0 + random.NextDouble() * 2.0, 1),
                Text = ReviewPhrases[random.Next(ReviewPhrases.Length)],
                CreatedAtUtc = created,
                UpdatedAtUtc = created
            });
        }

        return reviews;
    }

    internal static string BuildDescription(string title, string cityName, string categoryName, string subCategoryName) =>
        $"""
<p><strong>{title}</strong> provides {subCategoryName.ToLower()} services in {cityName}. The service is part of the {categoryName.ToLower()} category and is designed for users who want a clear, professional, and convenient experience.</p>
<p>Clients choose this listing because of strong communication, flexible scheduling, and a practical service approach. The page includes contacts, address, ratings, and reviews to help compare available options.</p>
<p>If you are looking for {subCategoryName.ToLower()} in {cityName}, this listing can be a useful option for quick selection and contact.</p>
""";

    internal static string MakeUniqueTitle(string title, int index) => $"{title} {index}";

    internal static string GeneratePhone(Random random) =>
        $"+380{random.Next(50, 99)}{random.Next(1000000, 9999999)}";

    internal static string GenerateSlug(string text) =>
        text
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace(",", "")
            .Replace(".", "")
            .Replace("/", "-")
            .Replace("&", "and")
            .Replace("--", "-");
}
