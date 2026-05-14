using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.ContactRequests;

public sealed class ContactRequestDetailVm
{
    public int Id { get; init; }
    public ContactRequestType Type { get; init; }
    public ContactRequestStatus Status { get; set; }

    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public string? CustomerEmail { get; init; }
    public string? CustomerCompany { get; init; }
    public string Message { get; init; } = string.Empty;

    public int? ListingId { get; init; }
    public string? ListingTitle { get; init; }

    [StringLength(1000)]
    public string? AdminNotes { get; set; }

    public DateTime CreatedAtUtc { get; init; }
    public DateTime? ProcessedAtUtc { get; init; }
}
