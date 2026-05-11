using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class MediatRInjection
{
    public static IServiceCollection AddUsersMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(Application.Users.Commands.CreateUser.CreateUserCommand).Assembly));

        return services;
    }
}
