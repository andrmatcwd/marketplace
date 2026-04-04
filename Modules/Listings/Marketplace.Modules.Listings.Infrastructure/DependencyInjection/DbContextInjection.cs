using System;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Listings.Infrastructure.DependencyInjection;

public static class DbContextInjection
{
    public static IServiceCollection AddListingsDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ListingsDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
