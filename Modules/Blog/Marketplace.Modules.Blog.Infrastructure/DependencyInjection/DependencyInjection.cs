using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Blog.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBlogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBlogDbContext(configuration);
        services.AddBlogRepositories();
        services.AddBlogMediatR();

        return services;
    }
}
