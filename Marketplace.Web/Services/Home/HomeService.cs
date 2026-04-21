using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Home;

public sealed class HomeService : IHomeService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICatalogVmMapper _catalogVmMapper;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public HomeService(
        ApplicationDbContext dbContext,
        ICatalogVmMapper catalogVmMapper,
        ICatalogUrlBuilder urlBuilder)
    {
        _dbContext = dbContext;
        _catalogVmMapper = catalogVmMapper;
        _urlBuilder = urlBuilder;
    }

    public async Task<HomePageVm> GetHomePageAsync(string culture, string? selectedCitySlug, CancellationToken cancellationToken)
    {
        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CityId == x.Id && l.IsPublished)
            })
            .Take(8)
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CategoryId == x.Id && l.IsPublished)
            })
            .Take(8)
            .ToListAsync(cancellationToken);

        var featuredListings = await _dbContext.Listings
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.Images)
            .OrderByDescending(x => x.ReviewsCount)
            .ThenByDescending(x => x.Rating)
            .Take(8)
            .ToListAsync(cancellationToken);

        return new HomePageVm
        {
            Culture = culture,
            SelectedCitySlug = string.IsNullOrWhiteSpace(selectedCitySlug)
                ? null
                : selectedCitySlug.Trim(),
            Hero = new HomeHeroVm
            {
                Title = "Знайдіть перевірені послуги у своєму місті",
                Subtitle = "Каталог компаній, спеціалістів і локальних сервісів з пошуком за містами, категоріями та напрямками.",
                SearchActionUrl = _urlBuilder.BuildCatalogUrl(culture),
                SearchPlaceholder = "Наприклад: стоматолог, клінінг, майстер ремонту",
                CityPlaceholder = "Оберіть місто"
            },
            CityOptions = cities
                .Select(x => _catalogVmMapper.MapFilterOption(x.Entity.Slug, x.Entity.Name))
                .ToList(),
            PopularCitiesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Популярні міста",
                    Description = "Оберіть місто та перегляньте доступні послуги."
                },
                Items = cities
                    .Select(x => _catalogVmMapper.MapCityCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            PopularCategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Популярні категорії",
                    Description = "Швидкий доступ до основних напрямків послуг."
                },
                Items = categories
                    .Select(x => _catalogVmMapper.MapCategoryCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            FeaturedListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Популярні пропозиції",
                    Description = "Добірка актуальних і популярних послуг."
                },
                Listings = featuredListings
                    .Select(x => _catalogVmMapper.MapListingCard(x, culture))
                    .ToList(),
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