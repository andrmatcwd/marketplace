using Marketplace.Modules.Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Blog.Infrastructure.Persistence.Configurations;

public sealed class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Excerpt).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.CoverImageUrl).HasMaxLength(2000);

        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.IsPublished);
        builder.HasIndex(x => x.CreatedAtUtc);
    }
}
