using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Seo;

public sealed class AbsoluteUrlBuilder : IAbsoluteUrlBuilder
{
    public string Build(HttpRequest request, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return $"{request.Scheme}://{request.Host}";
        }

        if (!relativePath.StartsWith('/'))
        {
            relativePath = "/" + relativePath;
        }

        return $"{request.Scheme}://{request.Host}{relativePath}";
    }
}