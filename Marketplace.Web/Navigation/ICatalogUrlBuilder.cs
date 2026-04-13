using System;

namespace Marketplace.Web.Navigation;

public interface ICatalogUrlBuilder
{
    string Build(
        string? culture = null,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null,
        string? listingSlug = null,
        int? listingId = null);

    string BuildCity(string? culture, string citySlug);

    string BuildCategory(string? culture, string citySlug, string categorySlug);

    string BuildSubCategory(string? culture, string citySlug, string categorySlug, string subCategorySlug);

    string BuildListing(
        string? culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string listingSlug,
        int listingId);
}
