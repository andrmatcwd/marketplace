using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("SubCategories");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Listings)
            .WithOne(x => x.SubCategory)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}