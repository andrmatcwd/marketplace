using Marketplace.Modules.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Users.Infrastructure.DependencyInjection;

public static class DbContextInjection
{
    public static IServiceCollection AddUsersDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
