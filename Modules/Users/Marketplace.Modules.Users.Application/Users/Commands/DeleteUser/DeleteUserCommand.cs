using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(int Id) : IRequest<Unit>;
