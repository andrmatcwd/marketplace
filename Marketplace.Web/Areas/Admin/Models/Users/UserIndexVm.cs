using Marketplace.Modules.Users.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.Users;

public class UserIndexVm
{
    public string? Search { get; set; }
    public UserStatus? Status { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public IReadOnlyCollection<UserListItemVm> Items { get; set; } = [];
}
