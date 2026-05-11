using Marketplace.Modules.Users.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string IdentityUserId,
    string DisplayName,
    string? Email,
    string? Phone,
    string? AvatarUrl,
    string? Bio,
    UserStatus Status
) : IRequest<Unit>;
