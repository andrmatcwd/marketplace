using Marketplace.Modules.Notifications.Application.ContactRequests.Dtos;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestById;

public sealed record GetContactRequestByIdQuery(int Id) : IRequest<ContactRequestDto?>;
