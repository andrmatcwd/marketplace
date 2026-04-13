using System;

namespace Marketplace.Web.Navigation;

public sealed class CatalogUrlBuilder : ICatalogUrlBuilder
{
    public string Build(
        string? culture = null,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null,
        string? listingSlug = null,
        int? listingId = null)
    {
        var parts = new List<string>();

        AddIfHasValue(parts, culture);
        AddIfHasValue(parts, citySlug);
        AddIfHasValue(parts, categorySlug);
        AddIfHasValue(parts, subCategorySlug);

        if (!string.IsNullOrWhiteSpace(listingSlug) && listingId.HasValue)
        {
            parts.Add($"{listingSlug}-{listingId.Value}");
        }
        else if (!string.IsNullOrWhiteSpace(listingSlug))
        {
            parts.Add(listingSlug);
        }
        else if (listingId.HasValue)
        {
            parts.Add(listingId.Value.ToString());
        }

        return "/" + string.Join("/", parts);
    }

    public string BuildCity(string? culture, string citySlug)
        => Build(culture: culture, citySlug: citySlug);

    public string BuildCategory(string? culture, string citySlug, string categorySlug)
        => Build(culture: culture, citySlug: citySlug, categorySlug: categorySlug);

    public string BuildSubCategory(string? culture, string citySlug, string categorySlug, string subCategorySlug)
        => Build(
            culture: culture,
            citySlug: citySlug,
            categorySlug: categorySlug,
            subCategorySlug: subCategorySlug);

    public string BuildListing(
        string? culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string listingSlug,
        int listingId)
        => Build(
            culture: culture,
            citySlug: citySlug,
            categorySlug: categorySlug,
            subCategorySlug: subCategorySlug,
            listingSlug: listingSlug,
            listingId: listingId);

    private static void AddIfHasValue(List<string> parts, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            parts.Add(value);
        }
    }
}
