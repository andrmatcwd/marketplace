using Marketplace.Modules.Notifications.Application.Notifications.Filters;
using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Notifications.Infrastructure.Persistence.Repositories;

public class NotificationRepository : BaseRepository<Notification, int>, INotificationRepository
{
    public NotificationRepository(NotificationsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IReadOnlyCollection<Notification> Items, int TotalCount)> GetByFilterAsync(
        NotificationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.RecipientId))
            query = query.Where(x => x.RecipientId == filter.RecipientId);

        if (filter.IsRead.HasValue)
            query = query.Where(x => x.IsRead == filter.IsRead.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(x => x.Title.Contains(filter.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Notification>> GetByRecipientIdAsync(string recipientId, CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking()
            .Where(x => x.RecipientId == recipientId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public Task<int> GetUnreadCountAsync(string recipientId, CancellationToken cancellationToken = default)
        => DbSet.CountAsync(x => x.RecipientId == recipientId && !x.IsRead, cancellationToken);

    public async Task MarkAllAsReadAsync(string recipientId, CancellationToken cancellationToken = default)
    {
        await DbSet
            .Where(x => x.RecipientId == recipientId && !x.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRead, true), cancellationToken);
    }
}
