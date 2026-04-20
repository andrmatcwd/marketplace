using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class SubCategoryPageVm
{
    public string Culture { get; set; } = "uk";

    public string SubCategoryName { get; set; } = string.Empty;
    public string SubCategorySlug { get; set; } = string.Empty;

    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }

    public string? CityName { get; set; }
    public string? CitySlug { get; set; }

    public PageHeroVm Hero { get; set; } = new();
    public SeoIntroVm SeoIntro { get; set; } = new();

    public ListingsSectionVm ListingsSection { get; set; } = new();
}