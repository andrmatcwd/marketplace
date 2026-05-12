using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class ListingSubscriptionConfiguration : IEntityTypeConfiguration<ListingSubscription>
{
    public void Configure(EntityTypeBuilder<ListingSubscription> builder)
    {
        builder.ToTable("ListingSubscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignedByUserId).HasMaxLength(150);
        builder.Property(x => x.RequestedByUserId).HasMaxLength(150);
        builder.Property(x => x.Notes).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<int>();

        builder.HasIndex(x => new { x.ListingId, x.Status });
        builder.HasIndex(x => x.ExpiresAt);

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Plan)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
