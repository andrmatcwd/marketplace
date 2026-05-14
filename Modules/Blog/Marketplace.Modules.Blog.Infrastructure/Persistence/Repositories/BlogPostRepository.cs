using Marketplace.Modules.Blog.Application.BlogPosts.Filters;
using Marketplace.Modules.Blog.Application.Repositories;
using Marketplace.Modules.Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Blog.Infrastructure.Persistence.Repositories;

public class BlogPostRepository : BaseRepository<BlogPost, int>, IBlogPostRepository
{
    public BlogPostRepository(BlogDbContext dbContext) : base(dbContext) { }

    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

    public async Task<(IReadOnlyCollection<BlogPost> Items, int TotalCount)> GetByFilterAsync(
        BlogPostFilter filter, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().AsQueryable();

        if (filter.IsPublished.HasValue)
            query = query.Where(x => x.IsPublished == filter.IsPublished.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.ToLower();
            query = query.Where(x =>
                x.Title.ToLower().Contains(search) ||
                x.Slug.ToLower().Contains(search) ||
                x.Excerpt.ToLower().Contains(search));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
