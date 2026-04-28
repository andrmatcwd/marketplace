using Microsoft.AspNetCore.Localization;

namespace Marketplace.Web.Localization;

public sealed class RouteSegmentRequestCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value;

        if (string.IsNullOrWhiteSpace(path))
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        var cultureSegment = segments[0].Trim().ToLowerInvariant();

        var culture = cultureSegment switch
        {
            "uk" => "uk-UA",
            "ru" => "ru-RU",
            _ => null
        };

        return culture is null
            ? Task.FromResult<ProviderCultureResult?>(null)
            : Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
    }
}