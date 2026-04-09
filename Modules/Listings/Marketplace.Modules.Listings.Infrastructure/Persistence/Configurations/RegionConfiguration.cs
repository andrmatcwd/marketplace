using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Locations)
            .WithOne(x => x.Region)
            .HasForeignKey(x => x.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}