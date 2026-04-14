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

        var culture = segments[0].Trim().ToLowerInvariant();

        if (culture is "uk" or "en")
        {
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
        }

        return Task.FromResult<ProviderCultureResult?>(null);
    }
}