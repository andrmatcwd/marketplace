using Marketplace.Modules.Notifications.Application.Common.Models;

namespace Marketplace.Modules.Notifications.Application.Notifications.Filters;

public class NotificationFilter : PaginationFilter
{
    public string? RecipientId { get; set; }
    public bool? IsRead { get; set; }
}
