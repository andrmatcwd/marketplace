using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Filters;

public sealed class ContactRequestFilter : PaginationFilter
{
    public string? Search { get; init; }
    public ContactRequestType? Type { get; init; }
    public ContactRequestStatus? Status { get; init; }
}
