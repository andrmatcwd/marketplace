namespace Marketplace.Web.Models.Common;

public class BreadcrumbItemVm
{
    public string Title { get; init; } = default!;
    public string? Url { get; init; }
    public bool IsCurrent { get; init; }
}