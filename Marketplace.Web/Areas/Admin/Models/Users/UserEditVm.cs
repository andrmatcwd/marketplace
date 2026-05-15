using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Users.Domain.Enums;
using Marketplace.Web.Authorization;

namespace Marketplace.Web.Areas.Admin.Models.Users;

public class UserEditVm
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    public UserStatus Status { get; set; }
    public string Role { get; set; } = AppRoles.User;

    public string? IdentityUserId { get; set; }
    public bool IsSelf { get; set; }
}
