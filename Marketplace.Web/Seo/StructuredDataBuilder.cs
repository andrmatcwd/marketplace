using System.Text.Json;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Seo;

public sealed class StructuredDataBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null
    };

    public string BuildItemList(IEnumerable<(string Name, string Url)> items)
    {
        var list = items
            .Select((x, index) => new Dictionary<string, object?>
            {
                ["@type"] = "ListItem",
                ["position"] = index + 1,
                ["name"] = x.Name,
                ["url"] = x.Url
            })
            .ToList();

        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "ItemList",
            ["itemListElement"] = list
        };

        return JsonSerializer.Serialize(payload, JsonOptions);
    }

    public string BuildListing(ListingDetailsPageVm model, string absoluteUrl, string? absoluteImageUrl = null)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Service",
            ["name"] = model.Title,
            ["description"] = model.ShortDescription ?? model.Description,
            ["url"] = absoluteUrl
        };

        if (!string.IsNullOrWhiteSpace(model.CategoryName))
        {
            payload["serviceType"] = model.CategoryName;
        }

        if (!string.IsNullOrWhiteSpace(absoluteImageUrl))
        {
            payload["image"] = absoluteImageUrl;
        }

        if (model.HasRating && model.ReviewsCount > 0)
        {
            payload["aggregateRating"] = new Dictionary<string, object?>
            {
                ["@type"] = "AggregateRating",
                ["ratingValue"] = model.Rating.ToString("0.0"),
                ["reviewCount"] = model.ReviewsCount
            };
        }

        if (!string.IsNullOrWhiteSpace(model.CityName) || !string.IsNullOrWhiteSpace(model.Address))
        {
            payload["areaServed"] = new Dictionary<string, object?>
            {
                ["@type"] = "City",
                ["name"] = model.CityName ?? model.Address
            };
        }

        if (model.Contact.HasAny)
        {
            payload["provider"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = model.Contact.ContactName ?? model.Title,
                ["telephone"] = model.Contact.Phone,
                ["email"] = model.Contact.Email,
                ["url"] = model.Contact.Website
            };
        }

        if (model.Reviews.Any())
        {
            payload["review"] = model.Reviews
                .Where(x => x.HasText)
                .Take(5)
                .Select(x => new Dictionary<string, object?>
                {
                    ["@type"] = "Review",
                    ["author"] = new Dictionary<string, object?>
                    {
                        ["@type"] = "Person",
                        ["name"] = x.AuthorName
                    },
                    ["reviewBody"] = x.Text,
                    ["reviewRating"] = new Dictionary<string, object?>
                    {
                        ["@type"] = "Rating",
                        ["ratingValue"] = x.Rating.ToString("0.0")
                    }
                })
                .ToList();
        }

        return JsonSerializer.Serialize(payload, JsonOptions);
    }

    public string BuildWebSite(string siteUrl, string searchTargetUrl)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "WebSite",
            ["name"] = "Marketplace",
            ["url"] = siteUrl,
            ["potentialAction"] = new Dictionary<string, object?>
            {
                ["@type"] = "SearchAction",
                ["target"] = new Dictionary<string, object?>
                {
                    ["@type"] = "EntryPoint",
                    ["urlTemplate"] = searchTargetUrl + "{search_term_string}"
                },
                ["query-input"] = "required name=search_term_string"
            }
        };

        return JsonSerializer.Serialize(payload, JsonOptions);
    }

    public string BuildBreadcrumbs(IEnumerable<(string Name, string Url)> items)
    {
        var list = items
            .Select((x, index) => new Dictionary<string, object?>
            {
                ["@type"] = "ListItem",
                ["position"] = index + 1,
                ["name"] = x.Name,
                ["item"] = x.Url
            })
            .ToList();

        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "BreadcrumbList",
            ["itemListElement"] = list
        };

        return JsonSerializer.Serialize(payload, JsonOptions);
    }
}