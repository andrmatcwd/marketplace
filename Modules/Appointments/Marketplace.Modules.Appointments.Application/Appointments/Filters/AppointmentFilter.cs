using Marketplace.Modules.Appointments.Application.Common.Models;
using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Modules.Appointments.Application.Appointments.Filters;

public sealed class AppointmentFilter : PaginationFilter
{
    public AppointmentStatus? Status { get; init; }
}
