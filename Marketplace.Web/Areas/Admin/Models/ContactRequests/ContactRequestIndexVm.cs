using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.ContactRequests;

public sealed class ContactRequestIndexVm
{
    public string? Search { get; init; }
    public ContactRequestType? Type { get; init; }
    public ContactRequestStatus? Status { get; init; }

    public IReadOnlyCollection<ContactRequestListItemVm> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int TotalPages { get; init; }
}
