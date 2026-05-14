using Marketplace.Modules.Appointments.Application.Appointments.Filters;
using Marketplace.Modules.Appointments.Application.Repositories;
using Marketplace.Modules.Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Appointments.Infrastructure.Persistence.Repositories;

internal sealed class AppointmentRepository : BaseRepository<Appointment, int>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentsDbContext context) : base(context) { }

    public async Task<(IReadOnlyCollection<Appointment> Items, int TotalCount)> GetByFilterAsync(
        AppointmentFilter filter, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.ToLower();
            query = query.Where(a =>
                a.BusinessName.ToLower().Contains(search) ||
                a.ContactName.ToLower().Contains(search)  ||
                a.Email.ToLower().Contains(search)        ||
                a.CityName.ToLower().Contains(search));
        }

        if (filter.Status.HasValue)
            query = query.Where(a => a.Status == filter.Status.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
