using System;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class MediatRInjection
{
    public static IServiceCollection AddListingsMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(Application.Listings.Commands.CreateListing.CreateListingCommand).Assembly));

        return services;
    }
}
