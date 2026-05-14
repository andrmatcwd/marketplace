using Marketplace.Modules.Notifications.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.CreateContactRequest;

public sealed record CreateContactRequestCommand(
    ContactRequestType Type,
    string CustomerName,
    string CustomerPhone,
    string? CustomerEmail,
    string? CustomerCompany,
    string Message,
    int? ListingId,
    string? ListingTitle
) : IRequest<int>;
