using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CatalogGatewayPageVm
{
    public string Culture { get; set; } = "uk";
    public string? SelectedCitySlug { get; set; }
    public string? ContinueToCityUrl { get; set; }

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();
    public SeoBottomVm? SeoBottom { get; set; } = new();

    public IReadOnlyCollection<FilterOptionVm> CityOptions { get; set; } = Array.Empty<FilterOptionVm>();
    public TaxonomySectionVm<CityCardVm> CitiesSection { get; set; } = new();
    public ListingsSectionVm FeaturedListingsSection { get; set; } = new();
}