using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Appointments.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAppointmentsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppointmentsDbContext(configuration);
        services.AddAppointmentsRepositories();
        services.AddAppointmentsMediatR();
        return services;
    }
}
