using Marketplace.Modules.Blog.Application.BlogPosts.Filters;
using Marketplace.Modules.Blog.Domain.Entities;

namespace Marketplace.Modules.Blog.Application.Repositories;

public interface IBlogPostRepository : IBaseRepository<BlogPost, int>
{
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<BlogPost> Items, int TotalCount)> GetByFilterAsync(
        BlogPostFilter filter, CancellationToken cancellationToken = default);
}
