using System;

namespace Marketplace.Web.Models.Common;

public class PaginationVm
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public int PreviousPage => CurrentPage - 1;
    public int NextPage => CurrentPage + 1;
}
