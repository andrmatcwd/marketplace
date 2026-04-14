using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CatalogIndexPageVm
{
    public string Culture { get; set; } = "uk";

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();

    public TaxonomySectionVm<CityCardVm> CitiesSection { get; set; } = new();
    public TaxonomySectionVm<CategoryCardVm> CategoriesSection { get; set; } = new();
    public ListingsSectionVm ListingsSection { get; set; } = new();
}