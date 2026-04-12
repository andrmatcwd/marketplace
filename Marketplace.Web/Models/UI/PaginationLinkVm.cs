namespace Marketplace.Web.Models.UI;

public class PaginationLinkVm
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }

    public string? PreviousUrl { get; init; }
    public string? NextUrl { get; init; }

    public bool HasPrevious => !string.IsNullOrWhiteSpace(PreviousUrl);
    public bool HasNext => !string.IsNullOrWhiteSpace(NextUrl);
}