using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Home;

public sealed class HomeService : IHomeService
{
    private readonly IMediator _mediator;
    private readonly ICatalogVmMapper _catalogVmMapper;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public HomeService(
        IMediator mediator,
        ICatalogVmMapper catalogVmMapper,
        ICatalogUrlBuilder urlBuilder)
    {
        _mediator = mediator;
        _catalogVmMapper = catalogVmMapper;
        _urlBuilder = urlBuilder;
    }

    public async Task<HomePageVm> GetHomePageAsync(string culture, string? selectedCitySlug, CancellationToken cancellationToken)
    {
        var citiesTask = _mediator.Send(new GetCatalogCitiesQuery(Take: 8), cancellationToken);
        var categoriesTask = _mediator.Send(new GetCatalogCategoriesQuery(Take: 8), cancellationToken);
        var featuredListingsTask = _mediator.Send(new GetFeaturedListingsQuery(8), cancellationToken);
        await Task.WhenAll(citiesTask, categoriesTask, featuredListingsTask);
        var cities = citiesTask.Result;
        var categories = categoriesTask.Result;
        var featuredListings = featuredListingsTask.Result;

        return new HomePageVm
        {
            Culture = culture,
            SelectedCitySlug = string.IsNullOrWhiteSpace(selectedCitySlug) ? null : selectedCitySlug.Trim(),
            Hero = new HomeHeroVm
            {
                Title = "Знайдіть перевірені послуги у своєму місті",
                Subtitle = "Каталог компаній, спеціалістів і локальних сервісів з пошуком за містами, категоріями та напрямками.",
                SearchActionUrl = _urlBuilder.BuildCatalogUrl(culture),
                SearchPlaceholder = "Наприклад: стоматолог, клінінг, майстер ремонту",
                CityPlaceholder = "Оберіть місто"
            },
            CityOptions = cities
                .Select(x => _catalogVmMapper.MapFilterOption(x.Slug, x.Name))
                .ToList(),
            PopularCitiesSection = new()
            {
                Header = new SectionHeaderVm { Title = "Популярні міста", Description = "Оберіть місто та перегляньте доступні послуги." },
                Items = cities.Select(x => _catalogVmMapper.MapCityCard(x, culture)).ToList()
            },
            PopularCategoriesSection = new()
            {
                Header = new SectionHeaderVm { Title = "Популярні категорії", Description = "Швидкий доступ до основних напрямків послуг." },
                Items = categories.Select(x => _catalogVmMapper.MapCategoryCard(x, culture)).ToList()
            },
            FeaturedListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Популярні пропозиції", Description = "Добірка актуальних і популярних послуг." },
                Listings = featuredListings.Select(x => _catalogVmMapper.MapListingCard(x, culture)).ToList(),
                ShowMobileFilterButton = false
            },
            SeoIntro = new SeoIntroVm
            {
                Title = "Про каталог послуг",
                Text = """
                    <p>Marketplace допомагає швидко знайти послуги у потрібному місті: від медицини та краси до побутових і локальних сервісів.</p>
                    <p>Оберіть місто, перегляньте категорії або скористайтеся пошуком, щоб знайти потрібну компанію чи спеціаліста.</p>
                    """
            }
        };
    }
}
