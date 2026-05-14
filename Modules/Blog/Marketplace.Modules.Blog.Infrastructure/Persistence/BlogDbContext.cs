using Marketplace.Modules.Blog.Domain.Entities;
using Marketplace.Modules.Blog.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Blog.Infrastructure.Persistence;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
    }
}
