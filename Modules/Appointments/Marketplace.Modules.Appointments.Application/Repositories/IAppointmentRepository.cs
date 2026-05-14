using Marketplace.Modules.Appointments.Application.Appointments.Filters;
using Marketplace.Modules.Appointments.Domain.Entities;

namespace Marketplace.Modules.Appointments.Application.Repositories;

public interface IAppointmentRepository : IBaseRepository<Appointment, int>
{
    Task<(IReadOnlyCollection<Appointment> Items, int TotalCount)> GetByFilterAsync(
        AppointmentFilter filter, CancellationToken cancellationToken = default);
}
