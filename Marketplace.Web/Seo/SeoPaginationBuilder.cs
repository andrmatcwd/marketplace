using Marketplace.Web.Models.Shared;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Seo;

public sealed class SeoPaginationBuilder
{
    public string? BuildPrev(HttpRequest request, PaginationVm pagination)
    {
        if (!pagination.HasPrevious)
        {
            return null;
        }

        return $"{request.Scheme}://{request.Host}{pagination.BuildUrl(pagination.PreviousPage)}";
    }

    public string? BuildNext(HttpRequest request, PaginationVm pagination)
    {
        if (!pagination.HasNext)
        {
            return null;
        }

        return $"{request.Scheme}://{request.Host}{pagination.BuildUrl(pagination.NextPage)}";
    }
}