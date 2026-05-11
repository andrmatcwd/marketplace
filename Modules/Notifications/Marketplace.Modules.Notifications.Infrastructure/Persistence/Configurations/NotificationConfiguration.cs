using Marketplace.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Notifications.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RecipientId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Body).HasMaxLength(1000);
        builder.Property(x => x.Url).HasMaxLength(500);
        builder.Property(x => x.Type).HasConversion<int>();

        builder.HasIndex(x => x.RecipientId);
        builder.HasIndex(x => x.IsRead);
    }
}
