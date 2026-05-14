using Marketplace.Modules.Appointments.Application.Appointments.Commands.CreateAppointment;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Appointments.Infrastructure.DependencyInjection;

internal static class MediatRInjection
{
    internal static IServiceCollection AddAppointmentsMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateAppointmentCommand).Assembly));
        return services;
    }
}
