using System.Text.Json;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class ListingRentalConfiguration : IEntityTypeConfiguration<ListingRental>
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public void Configure(EntityTypeBuilder<ListingRental> builder)
    {
        builder.ToTable("ListingRentals");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasMaxLength(100);
        builder.Property(x => x.Rooms).HasMaxLength(100);
        builder.Property(x => x.Area).HasMaxLength(100);
        builder.Property(x => x.Floor).HasMaxLength(100);

        builder.Property(x => x.Features)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOpts),
                v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new List<string>(),
                new ValueComparer<List<string>>(
                    (a, b) => a != null && b != null && a.SequenceEqual(b),
                    v => v.Aggregate(0, (h, s) => HashCode.Combine(h, s.GetHashCode())),
                    v => v.ToList()));

        builder.HasMany(x => x.RoomOptions)
            .WithOne(x => x.Rental)
            .HasForeignKey(x => x.RentalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
