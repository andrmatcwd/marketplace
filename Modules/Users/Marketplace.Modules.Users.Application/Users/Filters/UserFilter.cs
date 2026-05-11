using Marketplace.Modules.Users.Application.Common.Models;
using Marketplace.Modules.Users.Domain.Enums;

namespace Marketplace.Modules.Users.Application.Users.Filters;

public class UserFilter : PaginationFilter
{
    public UserStatus? Status { get; set; }
}
