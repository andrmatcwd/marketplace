using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.SendContactNotification;

public sealed record SendContactNotificationCommand(
    int ListingId,
    string ListingTitle,
    string? ListingPhone,
    string? ListingEmail,
    string? ListingAddress,
    string CustomerName,
    string CustomerPhone,
    string CustomerMessage
) : IRequest<Unit>;
