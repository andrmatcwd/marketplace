using Marketplace.Modules.Users.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.Users;

public class UserListItemVm
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public UserStatus Status { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
