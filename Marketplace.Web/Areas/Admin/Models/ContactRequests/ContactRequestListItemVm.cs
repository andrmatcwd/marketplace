using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.ContactRequests;

public sealed class ContactRequestListItemVm
{
    public int Id { get; init; }
    public ContactRequestType Type { get; init; }
    public ContactRequestStatus Status { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public string? CustomerCompany { get; init; }
    public string? ListingTitle { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
