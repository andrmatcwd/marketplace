using Marketplace.Web.Models.Cards;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CategoryPageVm
{
    public string Culture { get; set; } = "uk";

    public string CityName { get; set; } = string.Empty;
    public string CitySlug { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();
    public SeoBottomVm? SeoBottom { get; set; } = new();

    public TaxonomySectionVm<SubCategoryCardVm> SubCategoriesSection { get; set; } = new();
    public ListingsSectionVm ListingsSection { get; set; } = new();

    public bool HasSubCategories => SubCategoriesSection.Items != null && SubCategoriesSection.Items.Any();
    public bool HasListings => ListingsSection.Listings != null && ListingsSection.Listings.Any();
}