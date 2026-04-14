using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Seo;

public interface IAbsoluteUrlBuilder
{
    string Build(HttpRequest request, string relativePath);
}