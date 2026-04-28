using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Home;

public sealed class HomePageVm
{
    public string Culture { get; set; } = "uk";
    public string? SelectedCitySlug { get; set; }

    public HomeHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();
    public SeoBottomVm? SeoBottom { get; set; } = new();

    public IReadOnlyCollection<FilterOptionVm> CityOptions { get; set; } = Array.Empty<FilterOptionVm>();

    public TaxonomySectionVm<CityCardVm> PopularCitiesSection { get; set; } = new();
    public TaxonomySectionVm<CategoryCardVm> PopularCategoriesSection { get; set; } = new();
    public ListingsSectionVm FeaturedListingsSection { get; set; } = new();
}