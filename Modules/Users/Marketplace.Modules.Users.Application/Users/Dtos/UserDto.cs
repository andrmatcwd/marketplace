using Marketplace.Modules.Users.Domain.Enums;

namespace Marketplace.Modules.Users.Application.Users.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
