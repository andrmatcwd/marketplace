using Marketplace.Modules.Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Blog.Infrastructure.DependencyInjection;

public static class DbContextInjection
{
    public static IServiceCollection AddBlogDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
