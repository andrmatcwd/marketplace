using Marketplace.Modules.Blog.Application.Repositories;
using Marketplace.Modules.Blog.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Blog.Infrastructure.DependencyInjection;

public static class RepositoryInjection
{
    public static IServiceCollection AddBlogRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();

        return services;
    }
}
