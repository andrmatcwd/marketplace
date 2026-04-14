using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Home;

public sealed class HomeService : IHomeService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICatalogVmMapper _catalogVmMapper;

    public HomeService(
        ApplicationDbContext dbContext,
        ICatalogVmMapper catalogVmMapper)
    {
        _dbContext = dbContext;
        _catalogVmMapper = catalogVmMapper;
    }

    public async Task<HomePageVm> GetHomePageAsync(string culture, CancellationToken cancellationToken)
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
            .OrderByDescending(x => x.Rating)
            .ThenByDescending(x => x.ReviewsCount)
            .Take(6)
            .ToListAsync(cancellationToken);

        return new HomePageVm
        {
            Culture = culture,
            Hero = new PageHeroVm
            {
                Title = "Service catalog in your city",
                Description = "Find trusted services, companies, and specialists in a clean directory."
            },
            SeoIntro = new SeoIntroVm
            {
                Title = "About the catalog",
                Text = "<p>Marketplace helps users quickly find actual services by city, category, and subcategory.</p>"
            },
            PopularCitiesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Popular cities",
                    Description = "Choose a city to browse available services."
                },
                Items = cities
                    .Select(x => _catalogVmMapper.MapCityCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            PopularCategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Popular categories",
                    Description = "Quick access to the main service directions."
                },
                Items = categories
                    .Select(x => _catalogVmMapper.MapCategoryCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            FeaturedListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Featured services"
                },
                Listings = featuredListings
                    .Select(x => _catalogVmMapper.MapListingCard(x, culture))
                    .ToList(),
                ShowMobileFilterButton = false
            }
        };
    }
}