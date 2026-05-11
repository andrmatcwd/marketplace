using Marketplace.Modules.Users.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.EditUser;

public sealed record EditUserCommand(
    int Id,
    string DisplayName,
    string? Email,
    string? Phone,
    string? AvatarUrl,
    string? Bio,
    UserStatus Status
) : IRequest<Unit>;
