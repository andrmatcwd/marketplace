using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Modules.Blog.Infrastructure.DependencyInjection;

public static class MediatRInjection
{
    public static IServiceCollection AddBlogMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(Application.BlogPosts.Commands.CreateBlogPost.CreateBlogPostCommand).Assembly));

        return services;
    }
}
