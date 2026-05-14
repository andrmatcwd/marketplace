using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.DeleteContactRequest;

public sealed record DeleteContactRequestCommand(int Id) : IRequest<bool>;
