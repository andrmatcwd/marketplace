using Marketplace.Modules.Notifications.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.UpdateContactRequestStatus;

public sealed record UpdateContactRequestStatusCommand(
    int Id,
    ContactRequestStatus Status,
    string? AdminNotes
) : IRequest<bool>;
