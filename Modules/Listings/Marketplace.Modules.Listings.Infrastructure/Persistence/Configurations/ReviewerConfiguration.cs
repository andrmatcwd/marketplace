using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class ReviewerConfiguration : IEntityTypeConfiguration<Reviewer>
{
    public void Configure(EntityTypeBuilder<Reviewer> builder)
    {
        builder.ToTable("Reviewers");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Reviewer)
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}