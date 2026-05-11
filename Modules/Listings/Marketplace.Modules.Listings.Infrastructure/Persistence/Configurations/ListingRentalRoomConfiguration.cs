using System.Text.Json;
using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Configurations;

public sealed class ListingRentalRoomConfiguration : IEntityTypeConfiguration<ListingRentalRoom>
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public void Configure(EntityTypeBuilder<ListingRentalRoom> builder)
    {
        builder.ToTable("ListingRentalRooms");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Price).HasMaxLength(100);
        builder.Property(x => x.Area).HasMaxLength(100);
        builder.Property(x => x.Guests).HasMaxLength(100);
        builder.Property(x => x.Beds).HasMaxLength(200);

        var listComparer = new ValueComparer<List<string>>(
            (a, b) => a != null && b != null && a.SequenceEqual(b),
            v => v.Aggregate(0, (h, s) => HashCode.Combine(h, s.GetHashCode())),
            v => v.ToList());

        builder.Property(x => x.Amenities)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOpts),
                v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new List<string>(),
                listComparer);

        builder.Property(x => x.ImageUrls)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOpts),
                v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new List<string>(),
                listComparer);
    }
}
