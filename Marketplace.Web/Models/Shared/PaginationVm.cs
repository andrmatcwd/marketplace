using Microsoft.AspNetCore.WebUtilities;

namespace Marketplace.Web.Models.Shared;

public sealed class PaginationVm
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public string BaseUrl { get; set; } = string.Empty;

    public IReadOnlyDictionary<string, string?> Query { get; set; } =
        new Dictionary<string, string?>();

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public int PreviousPage => HasPrevious ? CurrentPage - 1 : 1;
    public int NextPage => HasNext ? CurrentPage + 1 : TotalPages;

    public IEnumerable<int> GetPages()
    {
        if (TotalPages <= 0)
        {
            yield break;
        }

        var start = Math.Max(1, CurrentPage - 2);
        var end = Math.Min(TotalPages, CurrentPage + 2);

        for (var i = start; i <= end; i++)
        {
            yield return i;
        }
    }

    public string BuildUrl(int page)
    {
        var query = Query.ToDictionary(x => x.Key, x => x.Value);
        query["page"] = page.ToString();

        return QueryHelpers.AddQueryString(BaseUrl, query!);
    }
}