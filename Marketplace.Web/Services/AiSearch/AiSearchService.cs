using System.Text;
using System.Text.Json;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using MediatR;
using OpenAI.Chat;

namespace Marketplace.Web.Services.AiSearch;

public sealed class AiSearchService : IAiSearchService
{
    private readonly IConfiguration _config;
    private readonly ISender _sender;
    private readonly ILogger<AiSearchService> _logger;

    public AiSearchService(IConfiguration config, ISender sender, ILogger<AiSearchService> logger)
    {
        _config = config;
        _sender = sender;
        _logger = logger;
    }

    public async Task<AiSearchResult> SearchAsync(string description, string culture, CancellationToken cancellationToken = default)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return new AiSearchResult(false, null, "OpenAI API key is not configured.");

        var cities = await _sender.Send(new GetCitiesByFilterQuery(new CityFilter { PageSize = 200 }), cancellationToken);
        var categories = await _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter { PageSize = 200 }), cancellationToken);

        var cityList = string.Join("\n", cities.Items.Select(c => $"  {c.Name} → {c.Slug}"));
        var categoryList = string.Join("\n", categories.Items
            .DistinctBy(c => c.Slug)
            .Select(c => $"  {c.Name} → {c.Slug}"));

        var systemPrompt = $$"""
            You are a search assistant for a local services marketplace.
            Your task: read the user's problem description and extract structured search parameters.

            Available cities (display name → URL slug):
            {{cityList}}

            Available service categories (display name → URL slug):
            {{categoryList}}

            Return ONLY a JSON object with exactly these three fields:
            {
              "search": "<1–3 keyword service terms, or null if the category is specific enough>",
              "citySlug": "<matched slug, or null if no city is mentioned>",
              "categorySlug": "<best matching slug, or null if nothing fits>"
            }

            Rules:
            - Match cities by meaning, not exact spelling. Examples: "Odessa" → "odesa", "Kiev" → "kyiv", "Харків" → "kharkiv"
            - "search" should be a short action/service term (e.g. "pipe repair", "tax advice"), not the full sentence
            - Set "search" to null when the category already captures the intent precisely
            - Always use slugs from the provided lists — never invent new ones
            - The user may write in Ukrainian, Russian, or English — return correct slugs regardless of input language
            - Return only raw JSON, no markdown code fences, no explanation

            Example:
            Input: "my bathroom pipe is leaking, I need a plumber in Kyiv"
            Output: {"search": "pipe repair", "citySlug": "kyiv", "categorySlug": "plumbing"}
            """;

        try
        {
            var client = new ChatClient("gpt-4o-mini", apiKey);
            var completion = await client.CompleteChatAsync(
            [
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(description)
            ], cancellationToken: cancellationToken);

            var json = completion.Value.Content[0].Text.Trim();

            // Strip markdown code fences if GPT wraps in ```json
            if (json.StartsWith("```"))
            {
                var start = json.IndexOf('\n') + 1;
                var end = json.LastIndexOf("```");
                if (end > start) json = json[start..end].Trim();
            }

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var search = root.TryGetProperty("search", out var s) && s.ValueKind == JsonValueKind.String ? s.GetString() : null;
            var citySlug = root.TryGetProperty("citySlug", out var c) && c.ValueKind == JsonValueKind.String ? c.GetString() : null;
            var categorySlug = root.TryGetProperty("categorySlug", out var cat) && cat.ValueKind == JsonValueKind.String ? cat.GetString() : null;

            var url = BuildRedirectUrl(culture, search, citySlug, categorySlug);
            return new AiSearchResult(true, url, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI search failed for description: {Description}", description);
            return new AiSearchResult(false, null, "AI search is temporarily unavailable.");
        }
    }

    private static string BuildRedirectUrl(string culture, string? search, string? citySlug, string? categorySlug)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(citySlug))
        {
            sb.Append($"/{culture}/{citySlug}");
            if (!string.IsNullOrEmpty(categorySlug))
                sb.Append($"/{categorySlug}");
        }
        else
        {
            sb.Append($"/{culture}/catalog");
        }

        var query = new List<string>();
        if (!string.IsNullOrEmpty(search))
            query.Add($"search={Uri.EscapeDataString(search)}");

        if (query.Count > 0)
            sb.Append('?').Append(string.Join('&', query));

        return sb.ToString();
    }
}
