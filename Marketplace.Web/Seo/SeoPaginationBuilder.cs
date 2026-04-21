using Marketplace.Web.Models.Shared;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Seo;

public sealed class SeoPaginationBuilder
{
    public string? BuildPrev(HttpRequest request, PaginationVm pagination)
    {
        if (string.IsNullOrWhiteSpace(pagination.PrevUrl))
        {
            return null;
        }

        return $"{request.Scheme}://{request.Host}{pagination.PrevUrl}";
    }

    public string? BuildNext(HttpRequest request, PaginationVm pagination)
    {
        if (string.IsNullOrWhiteSpace(pagination.NextUrl))
        {
            return null;
        }

        return $"{request.Scheme}://{request.Host}{pagination.NextUrl}";
    }
}