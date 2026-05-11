using Marketplace.Modules.Users.Application.Repositories;
using Marketplace.Modules.Users.Application.Users.Filters;
using Marketplace.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Users.Infrastructure.Persistence.Repositories;

public class AppUserRepository : BaseRepository<AppUser, int>, IAppUserRepository
{
    public AppUserRepository(UsersDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<AppUser?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId, cancellationToken);

    public async Task<(IReadOnlyCollection<AppUser> Items, int TotalCount)> GetByFilterAsync(
        UserFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(x => x.DisplayName.Contains(filter.Search) || (x.Email != null && x.Email.Contains(filter.Search)));

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
