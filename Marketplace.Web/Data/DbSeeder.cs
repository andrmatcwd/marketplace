using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            var category = subCategory.Category;

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
            listing.Rental = BuildRental(listing, category.Slug, random);
            listing.Vacancies = BuildVacancies(category.Slug, city.Name, random);

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

    // ── Rental ────────────────────────────────────────────────────────────────

    private static ListingRental BuildRental(Listing listing, string categorySlug, Random random)
    {
        var cfg = RentalCfg(categorySlug);
        var pickedRooms = cfg.RoomPool
            .OrderBy(_ => random.Next())
            .Take(random.Next(1, Math.Min(cfg.RoomPool.Length, 3) + 1))
            .Select(r => new ListingRentalRoom
            {
                Title = r.Title,
                Description = r.Description,
                Price = r.Price,
                Area = r.Area,
                Guests = r.Guests,
                Beds = r.Beds,
                Amenities = [..r.Amenities.OrderBy(_ => random.Next()).Take(random.Next(3, r.Amenities.Length + 1))],
                ImageUrls = ["/img/placeholders/listing-default.jpg"]
            })
            .ToList();

        return new ListingRental
        {
            Listing = listing,
            Price = cfg.Prices[random.Next(cfg.Prices.Length)],
            Rooms = $"{pickedRooms.Count} {cfg.UnitLabel}",
            Area = cfg.Areas[random.Next(cfg.Areas.Length)],
            Floor = cfg.Floors[random.Next(cfg.Floors.Length)],
            Features = [..cfg.Features.OrderBy(_ => random.Next()).Take(random.Next(3, cfg.Features.Length + 1))],
            RoomOptions = pickedRooms
        };
    }

    private sealed record RoomTpl(string Title, string Description, string Price, string Area, string Guests, string Beds, string[] Amenities);
    private sealed record RentalConfig(string[] Prices, string UnitLabel, string[] Areas, string[] Floors, string[] Features, RoomTpl[] RoomPool);

    private static RentalConfig RentalCfg(string categorySlug) => categorySlug switch
    {
        "medicine" => new RentalConfig(
            Prices: ["від 350 ₴ / прийом", "від 500 ₴ / прийом", "від 700 ₴ / прийом"],
            UnitLabel: "кабінети",
            Areas: ["від 12 м²", "від 18 м²", "від 25 м²"],
            Floors: ["1 поверх", "2 поверхи", "3 поверхи"],
            Features: ["Стерильне обладнання", "Приватний кабінет", "Wi-Fi", "Паркінг", "Доступне середовище", "Дитяча зона", "Онлайн-запис"],
            RoomPool:
            [
                new("Кабінет лікаря", "Стандартний кабінет для первинного прийому та консультацій.", "350 ₴ / прийом", "14 м²", "1 пацієнт", "Оглядова кушетка", ["Медичне обладнання", "Стерильні інструменти", "Wi-Fi", "Кондиціонер"]),
                new("Процедурна кімната", "Кімната для проведення медичних процедур та маніпуляцій.", "500 ₴ / процедура", "20 м²", "1 пацієнт", "Медичне крісло", ["Стерилізатор", "Кисневий концентратор", "Кондиціонер", "Wi-Fi"]),
                new("Діагностичний кабінет", "Обладнана зона для діагностичних досліджень.", "700 ₴ / прийом", "25 м²", "1 пацієнт", "Діагностичне крісло", ["УЗД апарат", "ЕКГ", "Wi-Fi", "Кондиціонер", "Принтер"]),
            ]
        ),
        "beauty" => new RentalConfig(
            Prices: ["від 250 ₴ / сеанс", "від 400 ₴ / сеанс", "від 600 ₴ / сеанс"],
            UnitLabel: "кабінети",
            Areas: ["від 10 м²", "від 15 м²", "від 20 м²"],
            Floors: ["1 поверх", "2 поверхи"],
            Features: ["Сучасне обладнання", "Стерильні інструменти", "Wi-Fi", "Зручний онлайн-запис", "Приватний кабінет", "Кондиціонер", "Паркінг"],
            RoomPool:
            [
                new("Кабінет косметолога", "Затишний кабінет для косметологічних процедур.", "400 ₴ / сеанс", "12 м²", "1 клієнт", "Косметологічне крісло", ["Косметологічне обладнання", "Стерильні інструменти", "Wi-Fi", "Кондиціонер"]),
                new("Зона перукарні", "Сучасне місце для стрижок, фарбування та укладки.", "250 ₴ / послуга", "10 м²", "1 клієнт", "Перукарське крісло", ["Фен", "Праска", "Wi-Fi", "Дзеркало з підсвіткою"]),
                new("Кабінет масажу", "Спокійний кабінет для розслаблюючого або лікувального масажу.", "500 ₴ / сеанс", "16 м²", "1 клієнт", "Масажний стіл", ["Аромадифузор", "Тепла підлога", "Wi-Fi", "Кондиціонер"]),
            ]
        ),
        "home-services" => new RentalConfig(
            Prices: ["від 400 ₴ / виклик", "від 600 ₴ / послуга", "від 1 000 ₴ / день"],
            UnitLabel: "зони обслуговування",
            Areas: ["обслуговуємо до 50 м²", "обслуговуємо до 100 м²", "обслуговуємо до 200 м²"],
            Floors: ["приватні будинки", "квартири", "офіси та комерційні приміщення"],
            Features: ["Виїзд до клієнта", "Власний інвентар", "Дезінфекційні засоби", "Швидкий виклик", "Фотозвіт", "Гарантія якості"],
            RoomPool:
            [
                new("Базове прибирання", "Стандартне прибирання приміщень будь-якого типу.", "400 ₴ / виклик", "до 50 м²", "до 3 кімнат", "—", ["Пилосос", "Швабра", "Мийні засоби", "Мікрофібра"]),
                new("Генеральне прибирання", "Повне прибирання з миттям вікон, духовки та всіх поверхонь.", "900 ₴ / день", "до 100 м²", "до 5 кімнат", "—", ["Пилосос", "Парогенератор", "Мийні засоби", "Скребки"]),
                new("Прибирання після ремонту", "Прибирання від будівельного пилу, клею, фарби та сміття.", "1 200 ₴ / день", "до 100 м²", "до 5 кімнат", "—", ["Промисловий пилосос", "Мийні засоби", "Скребки", "Захисні рукавички"]),
            ]
        ),
        "education" => new RentalConfig(
            Prices: ["від 300 ₴ / урок", "від 450 ₴ / заняття", "від 1 500 ₴ / курс"],
            UnitLabel: "навчальні класи",
            Areas: ["від 15 м²", "від 25 м²", "від 40 м²"],
            Floors: ["1 поверх", "2 поверхи"],
            Features: ["Інтерактивна дошка", "Проектор", "Wi-Fi", "Навчальні матеріали", "Гнучкий розклад", "Онлайн-формат", "Сертифікат"],
            RoomPool:
            [
                new("Індивідуальний клас", "Затишний клас для занять один на один з викладачем.", "300 ₴ / урок", "15 м²", "1–2 учні", "Навчальний стіл", ["Дошка", "Маркери", "Wi-Fi", "Навчальні матеріали"]),
                new("Груповий клас", "Клас для групових занять до 8 осіб.", "450 ₴ / заняття", "30 м²", "до 8 учнів", "Парти", ["Проектор", "Дошка", "Wi-Fi", "Навчальні матеріали", "Кондиціонер"]),
                new("Конференц-зал", "Зал для лекцій, презентацій та великих групових занять.", "1 500 ₴ / курс", "50 м²", "до 20 осіб", "Аудиторні місця", ["Проектор", "Мікрофон", "Wi-Fi", "Дошка", "Кондиціонер"]),
            ]
        ),
        "legal" => new RentalConfig(
            Prices: ["від 500 ₴ / година", "від 800 ₴ / консультація", "від 3 000 ₴ / справа"],
            UnitLabel: "кабінети",
            Areas: ["від 12 м²", "від 20 м²", "від 35 м²"],
            Floors: ["1 поверх", "2 поверхи", "3 поверхи"],
            Features: ["Конфіденційність", "Приватний кабінет", "Wi-Fi", "Нотаріус у штаті", "Онлайн-консультація", "Запис відео-дзвінку", "Юридична бібліотека"],
            RoomPool:
            [
                new("Кабінет консультанта", "Приватний кабінет для правових консультацій.", "500 ₴ / година", "14 м²", "1–2 особи", "Переговорний стіл", ["Wi-Fi", "Принтер", "Сейф", "Кондиціонер"]),
                new("Переговорна кімната", "Кімната для зустрічей сторін та підписання договорів.", "800 ₴ / консультація", "25 м²", "до 6 осіб", "Конференц-стіл", ["Проектор", "Wi-Fi", "Принтер", "Кондиціонер"]),
                new("Нотаріальний кабінет", "Кабінет для нотаріального засвідчення документів.", "600 ₴ / дія", "16 м²", "1–3 особи", "Робочий стіл", ["Нотаріальна печатка", "Wi-Fi", "Принтер", "Сейф"]),
            ]
        ),
        "auto-services" => new RentalConfig(
            Prices: ["від 800 ₴ / обслуговування", "від 1 200 ₴ / ремонт", "від 2 500 ₴ / день"],
            UnitLabel: "автобокси",
            Areas: ["від 30 м²", "від 50 м²", "від 80 м²"],
            Floors: ["1 поверх (бокси)", "критий паркінг"],
            Features: ["Сучасна діагностика", "Підйомники", "Wi-Fi", "Зона очікування", "Гарантія на роботи", "Безготівкова оплата", "Евакуатор"],
            RoomPool:
            [
                new("Стандартний бокс", "Бокс для технічного обслуговування та поточного ремонту.", "800 ₴ / обслуговування", "35 м²", "1 авто", "Підйомник", ["Підйомник", "Набір інструментів", "Компресор", "Wi-Fi"]),
                new("Діагностичний бокс", "Бокс з повним діагностичним обладнанням.", "1 200 ₴ / ремонт", "50 м²", "1 авто", "Підйомник + стенд", ["Діагностичний сканер", "Підйомник", "Стенд розвал-сходження", "Wi-Fi"]),
                new("Детейлінг-зона", "Зона для хімчистки, полірування та захисного покриття.", "2 500 ₴ / день", "60 м²", "1–2 авто", "Спеціалізоване місце", ["Парогенератор", "Полірувальна машина", "Хімчистка", "Захисне покриття"]),
            ]
        ),
        "event-services" => new RentalConfig(
            Prices: ["від 2 000 ₴ / захід", "від 5 000 ₴ / захід", "від 15 000 ₴ / захід"],
            UnitLabel: "зали та зони",
            Areas: ["від 50 м²", "від 100 м²", "від 200 м²"],
            Floors: ["1 поверх", "2 поверхи", "окремий поверх"],
            Features: ["Власна кухня", "Звукове обладнання", "Сценічне освітлення", "Wi-Fi", "Паркінг", "Фотобудка", "Декорування"],
            RoomPool:
            [
                new("Банкетна зала", "Просторий зал для банкетів, весіль та корпоративів.", "5 000 ₴ / захід", "120 м²", "до 60 гостей", "Банкетні столи", ["Звукова система", "Освітлення", "Wi-Fi", "Кліматизація", "Власна кухня"]),
                new("Зона для фотозйомки", "Обладнана студія з реквізитом та фонами для зйомки.", "2 000 ₴ / захід", "40 м²", "до 15 осіб", "Студійне крісло", ["Спалах", "Фони", "Реквізит", "Wi-Fi"]),
                new("Конференц-майданчик", "Зона для корпоративних заходів, презентацій, тренінгів.", "3 500 ₴ / захід", "80 м²", "до 40 осіб", "Аудиторні місця", ["Проектор", "Мікрофон", "Wi-Fi", "Звукова система", "Кліматизація"]),
            ]
        ),
        _ => new RentalConfig( // fitness (default)
            Prices: ["від 200 ₴ / заняття", "від 800 ₴ / місяць", "від 2 500 ₴ / квартал"],
            UnitLabel: "зали",
            Areas: ["від 80 м²", "від 150 м²", "від 300 м²"],
            Floors: ["1 поверх", "2 поверхи"],
            Features: ["Роздягальня", "Душ", "Wi-Fi", "Паркінг", "Сауна", "Сучасне обладнання", "Персональний тренер"],
            RoomPool:
            [
                new("Тренажерний зал", "Великий зал з кардіо та силовим обладнанням.", "200 ₴ / заняття", "200 м²", "до 30 осіб", "Тренажери", ["Кардіообладнання", "Силові тренажери", "Wi-Fi", "Душ", "Роздягальня"]),
                new("Зала для групових занять", "Простора зала для йоги, пілатесу, аеробіки.", "180 ₴ / заняття", "80 м²", "до 20 осіб", "Килимки", ["Дзеркала", "Килимки", "Wi-Fi", "Кондиціонер", "Звукова система"]),
                new("Боксерська зала", "Зала з рингом та обладнанням для бойових мистецтв.", "250 ₴ / заняття", "120 м²", "до 15 осіб", "Ринг", ["Ринг", "Груші", "Wi-Fi", "Роздягальня", "Захисне спорядження"]),
            ]
        )
    };

    // ── Vacancies ─────────────────────────────────────────────────────────────

    private static List<ListingVacancy> BuildVacancies(string categorySlug, string cityName, Random random)
    {
        var pool = VacancyPool(categorySlug, cityName);
        return [..pool.OrderBy(_ => random.Next()).Take(random.Next(1, Math.Min(pool.Length, 3) + 1))];
    }

    private static ListingVacancy[] VacancyPool(string categorySlug, string cityName) => categorySlug switch
    {
        "medicine" =>
        [
            new() { Title = "Лікар загальної практики", Description = "Ведення прийому, консультування пацієнтів, ведення медичних карток.", EmploymentType = "Повна зайнятість", SalaryText = "від 25 000 грн", LocationText = cityName },
            new() { Title = "Медична сестра", Description = "Підготовка кабінету, виконання процедур, супровід лікаря.", EmploymentType = "Позмінно", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор клініки", Description = "Запис пацієнтів, зустріч гостей, робота з касою та документами.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
            new() { Title = "Санітар", Description = "Підтримка чистоти у приміщеннях, дезінфекція кабінетів.", EmploymentType = "Часткова зайнятість", SalaryText = "від 10 000 грн", LocationText = cityName },
        ],
        "beauty" =>
        [
            new() { Title = "Косметолог", Description = "Проведення косметологічних процедур, консультування клієнтів.", EmploymentType = "Повна зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Перукар-стиліст", Description = "Стрижки, фарбування, укладання для жінок та чоловіків.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Масажист", Description = "Релаксаційний та лікувальний масаж за записом.", EmploymentType = "Позмінно", SalaryText = "від 17 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор салону", Description = "Запис клієнтів, робота з касою, зустріч відвідувачів.", EmploymentType = "Повна зайнятість", SalaryText = "від 13 000 грн", LocationText = cityName },
        ],
        "home-services" =>
        [
            new() { Title = "Прибиральник / Прибиральниця", Description = "Прибирання квартир, будинків та офісів за графіком.", EmploymentType = "Позмінно", SalaryText = "від 12 000 грн", LocationText = cityName },
            new() { Title = "Сантехнік", Description = "Монтаж і ремонт сантехніки, усунення аварійних ситуацій.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Електрик", Description = "Монтаж і ремонт електропроводки, підключення обладнання.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Менеджер замовлень", Description = "Прийом заявок, координація бригад, спілкування з клієнтами.", EmploymentType = "Повна зайнятість", SalaryText = "від 15 000 грн", LocationText = cityName },
        ],
        "education" =>
        [
            new() { Title = "Викладач англійської мови", Description = "Проведення індивідуальних та групових уроків англійської.", EmploymentType = "Часткова зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Репетитор з математики", Description = "Підготовка учнів до ЗНО та ДПА з математики.", EmploymentType = "Часткова зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Тренер з програмування", Description = "Навчання основ Python, Web-розробки, алгоритмів.", EmploymentType = "Повна зайнятість", SalaryText = "від 25 000 грн", LocationText = cityName },
            new() { Title = "Менеджер курсів", Description = "Запис студентів, ведення розкладу, комунікація з викладачами.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
        ],
        "legal" =>
        [
            new() { Title = "Юрист-консультант", Description = "Правові консультації фізичних та юридичних осіб.", EmploymentType = "Повна зайнятість", SalaryText = "від 28 000 грн", LocationText = cityName },
            new() { Title = "Нотаріус / Помічник нотаріуса", Description = "Засвідчення документів, ведення нотаріальних дій.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Юридичний асистент", Description = "Ведення документів, підготовка договорів, підтримка юристів.", EmploymentType = "Повна зайнятість", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор офісу", Description = "Запис клієнтів, зустріч гостей, ведення архіву документів.", EmploymentType = "Повна зайнятість", SalaryText = "від 13 000 грн", LocationText = cityName },
        ],
        "auto-services" =>
        [
            new() { Title = "Автомеханік", Description = "Діагностика та ремонт автомобілів різних марок.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Майстер шиномонтажу", Description = "Шиномонтаж, балансування, продаж шин.", EmploymentType = "Позмінно", SalaryText = "від 15 000 грн", LocationText = cityName },
            new() { Title = "Детейлер", Description = "Хімчистка, полірування, нанесення захисних покриттів.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор автосервісу", Description = "Прийом замовлень, запис клієнтів, видача авто.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
        ],
        "event-services" =>
        [
            new() { Title = "Фотограф", Description = "Зйомка весіль, корпоративів, дитячих свят та портретів.", EmploymentType = "Фріланс / за заявками", SalaryText = "від 2 500 грн / захід", LocationText = cityName },
            new() { Title = "Менеджер заходів", Description = "Планування та організація заходів, координація команди.", EmploymentType = "Повна зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Кейтеринг-менеджер", Description = "Організація харчування на заходах, робота з постачальниками.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Декоратор / Флорист", Description = "Оформлення залів квітами, тканинами, декоративними елементами.", EmploymentType = "Фріланс / за заявками", SalaryText = "від 1 500 грн / захід", LocationText = cityName },
        ],
        _ => // fitness
        [
            new() { Title = "Персональний тренер", Description = "Розробка програм тренувань та індивідуальна робота з клієнтами.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Інструктор групових занять", Description = "Проведення занять з йоги, пілатесу, аеробіки.", EmploymentType = "Часткова зайнятість", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор залу", Description = "Запис клієнтів, контроль входу, допомога відвідувачам.", EmploymentType = "Позмінно", SalaryText = "від 13 000 грн", LocationText = cityName },
            new() { Title = "Тренер з бойових мистецтв", Description = "Ведення груп з боксу, MMA або самооборони.", EmploymentType = "Часткова зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
        ]
    };

    private sealed record CitySeed(string Name, string Slug, double Latitude, double Longitude);
}
