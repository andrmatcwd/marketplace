using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Listings;

public sealed class ListingService : IListingService
{
    private readonly IMediator _mediator;
    private readonly IListingVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public ListingService(
        IMediator mediator,
        IListingVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _mediator = mediator;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
    }

    public async Task<ListingDetailsPageVm?> GetDetailsPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        int id,
        CancellationToken cancellationToken)
    {
        var dto = await _mediator.Send(new GetListingDetailsQuery(id), cancellationToken);
        if (dto is null) return null;

        var related = await _mediator.Send(
            new GetRelatedListingsQuery(dto.Id, dto.CityId, dto.SubCategoryId), cancellationToken);

        var relatedListings = related.Select(x => _mapper.MapRelatedListing(x, culture)).ToList();

        var vm = _mapper.MapDetails(dto, culture, relatedListings);

        vm.Breadcrumbs = _breadcrumbBuilder.BuildListing(
            culture,
            dto.Title,
            dto.CityName,
            dto.CitySlug,
            dto.CategoryName,
            dto.CategorySlug,
            dto.SubCategoryName,
            dto.SubCategorySlug);

        vm.Rental = new RentalDetailsVm
        {
            Price = "від 1 200 ₴ / доба",
            Rooms = "4 номери",
            Area = "від 18 м²",
            Floor = "2 поверхи",
            Features = ["Wi-Fi", "Паркінг", "Можна з тваринами", "Кондиціонер", "Кухня"],
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
                    Amenities = ["Wi-Fi", "Душ", "Телевізор", "Кондиціонер"]
                }
            ]
        };

        vm.Vacancies =
        [
            new ListingVacancyVm
            {
                Title = "Адміністратор рецепції",
                Description = "Прийом гостей, бронювання номерів, робота з клієнтами.",
                EmploymentType = "Повна зайнятість",
                SalaryText = "від 15 000 грн",
                LocationText = dto.CityName
            },
            new ListingVacancyVm
            {
                Title = "Покоївка",
                Description = "Прибирання номерів, підтримка чистоти та порядку.",
                EmploymentType = "Позмінно",
                SalaryText = "від 10 000 грн",
                LocationText = dto.CityName
            }
        ];

        return vm;
    }
}
