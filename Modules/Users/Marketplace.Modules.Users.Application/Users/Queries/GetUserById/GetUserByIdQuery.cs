using Marketplace.Modules.Users.Application.Users.Dtos;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(int Id) : IRequest<UserDto?>;
