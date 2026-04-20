using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Seo;

public sealed class HreflangBuilder
{
    public IReadOnlyCollection<HreflangLink> Build(HttpRequest request, string canonicalPath)
    {
        string BuildUrl(string culture)
        {
            var path = canonicalPath.TrimStart('/');
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (segments.Count > 0 && (segments[0] == "uk" || segments[0] == "en"))
            {
                segments[0] = culture;
            }
            else
            {
                segments.Insert(0, culture);
            }

            return $"{request.Scheme}://{request.Host}/" + string.Join("/", segments);
        }

        return new List<HreflangLink>
        {
            new() { Lang = "uk", Url = BuildUrl("uk") },
            new() { Lang = "en", Url = BuildUrl("en") },
            new() { Lang = "x-default", Url = BuildUrl("uk") }
        };
    }
}