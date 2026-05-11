using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Navigation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Listings;

public sealed class ListingService : IListingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IListingVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public ListingService(
        ApplicationDbContext dbContext,
        IListingVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
    }

    public async Task<ListingDetailsPageVm?> GetDetailsPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.City)
            .Include(x => x.Images)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.IsPublished && x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        if (entity.City is null || entity.Category is null || entity.SubCategory is null)
        {
            return null;
        }

        var relatedEntities = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.Images)
            .Where(x =>
                x.IsPublished &&
                x.Id != entity.Id &&
                x.CityId == entity.CityId &&
                x.SubCategoryId == entity.SubCategoryId)
            .OrderByDescending(x => x.Rating)
            .ThenByDescending(x => x.ReviewsCount)
            .Take(6)
            .ToListAsync(cancellationToken);

        var relatedListings = relatedEntities
            .Select(x => _mapper.MapRelatedListing(x, culture))
            .ToList();

        var vm = _mapper.MapDetails(entity, culture, relatedListings);

        vm.Breadcrumbs = _breadcrumbBuilder.BuildListing(
            culture,
            entity.Title,
            entity.City.Name,
            entity.City.Slug,
            entity.Category.Name,
            entity.Category.Slug,
            entity.SubCategory.Name,
            entity.SubCategory.Slug);

        vm.Rental = new RentalDetailsVm
        {
            Price = "від 1 200 ₴ / доба",
            Rooms = "4 номери",
            Area = "від 18 м²",
            Floor = "2 поверхи",
            Features =
            [
                "Wi-Fi",
                "Паркінг",
                "Можна з тваринами",
                "Кондиціонер",
                "Кухня"
            ],
            RoomOptions =
            [
                new RentalRoomVm
                {
                    Title = "Стандартний номер",
                    Description = "Затишний номер для короткострокового проживання.",
                    ImageUrls =
                    [
                        "/uploads/rooms/standard-1.jpg",
                        "/uploads/rooms/standard-2.jpg",
                        "/uploads/rooms/standard-3.jpg",
                        "/uploads/rooms/standard-4.jpg"
                    ],
                    Price = "1 200 ₴ / доба",
                    Area = "18 м²",
                    Guests = "2 гості",
                    Beds = "1 двоспальне ліжко",
                    Amenities =
                    [
                        "Wi-Fi",
                        "Душ",
                        "Телевізор",
                        "Кондиціонер"
                    ]
                },
                new RentalRoomVm
                {
                    Title = "Стандартний номер",
                    Description = "Затишний номер для короткострокового проживання.",
                    ImageUrls =
                    [
                        "/uploads/rooms/standard-1.jpg",
                        "/uploads/rooms/standard-2.jpg",
                        "/uploads/rooms/standard-3.jpg",
                        "/uploads/rooms/standard-4.jpg"
                    ],
                    Price = "1 200 ₴ / доба",
                    Area = "18 м²",
                    Guests = "2 гості",
                    Beds = "1 двоспальне ліжко",
                    Amenities =
                    [
                        "Wi-Fi",
                        "Душ",
                        "Телевізор",
                        "Кондиціонер"
                    ]
                }
            ]
        };

        vm.Vacancies = new List<ListingVacancyVm>
        {
            new()
            {
                Title = "Адміністратор рецепції",
                Description = "Прийом гостей, бронювання номерів, робота з клієнтами.",
                EmploymentType = "Повна зайнятість",
                SalaryText = "від 15 000 грн",
                LocationText = "listing.CityName" // або просто "Коломия"
            },
            new()
            {
                Title = "Покоївка",
                Description = "Прибирання номерів, підтримка чистоти та порядку.",
                EmploymentType = "Позмінно",
                SalaryText = "від 10 000 грн",
                LocationText = "e.CityName"
            },
            new()
            {
                Title = "Менеджер з бронювань",
                Description = "Обробка заявок, комунікація з клієнтами, онлайн-бронювання.",
                EmploymentType = "Remote / Часткова зайнятість",
                SalaryText = "договірна",
                LocationText = "Віддалено"
            }
        };

        return vm;
    }
}