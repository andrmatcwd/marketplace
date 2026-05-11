using Marketplace.Modules.Users.Application.Common.Models;
using Marketplace.Modules.Users.Application.Users.Dtos;
using Marketplace.Modules.Users.Application.Users.Filters;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Queries.GetUsersByFilter;

public sealed record GetUsersByFilterQuery(UserFilter Filter) : IRequest<PagedResult<UserDto>>;
