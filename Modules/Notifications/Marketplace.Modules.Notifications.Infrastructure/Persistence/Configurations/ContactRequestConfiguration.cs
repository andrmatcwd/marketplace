using Marketplace.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Notifications.Infrastructure.Persistence.Configurations;

public sealed class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.ToTable("ContactRequests");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasConversion<int>().IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();

        builder.Property(x => x.CustomerName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.CustomerPhone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.CustomerEmail).HasMaxLength(200);
        builder.Property(x => x.CustomerCompany).HasMaxLength(200);
        builder.Property(x => x.Message).HasMaxLength(2000).IsRequired();

        builder.Property(x => x.ListingTitle).HasMaxLength(300);
        builder.Property(x => x.AdminNotes).HasMaxLength(1000);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.CreatedAtUtc);
    }
}
