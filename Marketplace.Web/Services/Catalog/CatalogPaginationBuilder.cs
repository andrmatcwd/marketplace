using System.Text;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogPaginationBuilder : ICatalogPaginationBuilder
{
    public PaginationVm Build(
        CatalogFilterVm filter,
        int totalItems,
        Func<int, string> buildPath)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

        if (totalPages <= 1)
        {
            return new PaginationVm
            {
                CurrentPage = filter.Page,
                TotalPages = totalPages
            };
        }

        var pages = BuildPageWindow(filter.Page, totalPages);

        var items = pages.Select(page =>
        {
            if (page is null)
            {
                return new PaginationItemVm
                {
                    IsEllipsis = true
                };
            }

            return new PaginationItemVm
            {
                Page = page.Value,
                Url = AppendQuery(buildPath(page.Value), filter),
                IsActive = page.Value == filter.Page,
                IsEllipsis = false
            };
        }).ToList();

        return new PaginationVm
        {
            CurrentPage = filter.Page,
            TotalPages = totalPages,
            PrevUrl = filter.Page > 1
                ? AppendQuery(buildPath(filter.Page - 1), filter)
                : null,
            NextUrl = filter.Page < totalPages
                ? AppendQuery(buildPath(filter.Page + 1), filter)
                : null,
            Items = items
        };
    }

    private static IReadOnlyCollection<int?> BuildPageWindow(int currentPage, int totalPages)
    {
        var pages = new List<int?>();

        if (totalPages <= 7)
        {
            for (var i = 1; i <= totalPages; i++)
            {
                pages.Add(i);
            }

            return pages;
        }

        pages.Add(1);

        var start = Math.Max(2, currentPage - 1);
        var end = Math.Min(totalPages - 1, currentPage + 1);

        if (start > 2)
        {
            pages.Add(null);
        }

        for (var i = start; i <= end; i++)
        {
            pages.Add(i);
        }

        if (end < totalPages - 1)
        {
            pages.Add(null);
        }

        pages.Add(totalPages);

        return pages;
    }

    private static string AppendQuery(string basePath, CatalogFilterVm filter)
    {
        var query = new Dictionary<string, string?>();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query["search"] = filter.Search;
        }

        if (!string.IsNullOrWhiteSpace(filter.Sort))
        {
            query["sort"] = filter.Sort;
        }

        if (filter.PageSize > 0 && filter.PageSize != 12)
        {
            query["pageSize"] = filter.PageSize.ToString();
        }

        if (query.Count == 0)
        {
            return basePath;
        }

        var builder = new StringBuilder(basePath);
        builder.Append('?');

        var isFirst = true;
        foreach (var pair in query)
        {
            if (string.IsNullOrWhiteSpace(pair.Value))
            {
                continue;
            }

            if (!isFirst)
            {
                builder.Append('&');
            }

            builder.Append(Uri.EscapeDataString(pair.Key));
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(pair.Value));

            isFirst = false;
        }

        return builder.ToString();
    }
}