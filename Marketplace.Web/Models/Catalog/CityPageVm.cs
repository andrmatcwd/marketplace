using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CityPageVm
{
    public string Culture { get; set; } = "uk";

    public string CityName { get; set; } = string.Empty;
    public string CitySlug { get; set; } = string.Empty;

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();
    public SeoBottomVm? SeoBottom { get; set; } = new();

    public TaxonomySectionVm<CategoryCardVm> CategoriesSection { get; set; } = new();
    public TaxonomySectionVm<SubCategoryCardVm> PopularSubCategoriesSection { get; set; } = new();
    public ListingsSectionVm ListingsSection { get; set; } = new();
}