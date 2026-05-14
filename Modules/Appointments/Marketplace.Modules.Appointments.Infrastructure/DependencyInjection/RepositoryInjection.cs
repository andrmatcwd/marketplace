using Marketplace.Modules.Appointments.Application.Repositories;
using Marketplace.Modules.Appointments.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Appointments.Infrastructure.DependencyInjection;

internal static class RepositoryInjection
{
    internal static IServiceCollection AddAppointmentsRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        return services;
    }
}
