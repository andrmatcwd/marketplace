using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CategoryPageVm
{
    public string Culture { get; set; } = "uk";

    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;

    public string? CityName { get; set; }
    public string? CitySlug { get; set; }

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();

    public TaxonomySectionVm<SubCategoryCardVm> SubCategoriesSection { get; set; } = new();
    public ListingsSectionVm ListingsSection { get; set; } = new();
}