using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.ContactRequests.Dtos;
using Marketplace.Modules.Notifications.Application.ContactRequests.Filters;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestsByFilter;

public sealed record GetContactRequestsByFilterQuery(ContactRequestFilter Filter)
    : IRequest<PagedResult<ContactRequestDto>>;
