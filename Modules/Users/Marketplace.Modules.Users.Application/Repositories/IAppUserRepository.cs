using Marketplace.Modules.Users.Application.Users.Filters;
using Marketplace.Modules.Users.Domain.Entities;

namespace Marketplace.Modules.Users.Application.Repositories;

public interface IAppUserRepository : IBaseRepository<AppUser, int>
{
    Task<AppUser?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<AppUser> Items, int TotalCount)> GetByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default);
}
