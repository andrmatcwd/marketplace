using Marketplace.Modules.Notifications.Application.ContactRequests.Filters;
using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Notifications.Infrastructure.Persistence.Repositories;

public sealed class ContactRequestRepository
    : BaseRepository<ContactRequest, int>, IContactRequestRepository
{
    public ContactRequestRepository(NotificationsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IReadOnlyCollection<ContactRequest> Items, int TotalCount)> GetByFilterAsync(
        ContactRequestFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (filter.Type.HasValue)
            query = query.Where(x => x.Type == filter.Type.Value);

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim().ToLower();
            query = query.Where(x =>
                x.CustomerName.ToLower().Contains(term) ||
                x.CustomerPhone.Contains(term) ||
                (x.CustomerEmail != null && x.CustomerEmail.ToLower().Contains(term)) ||
                (x.CustomerCompany != null && x.CustomerCompany.ToLower().Contains(term)) ||
                (x.ListingTitle != null && x.ListingTitle.ToLower().Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
