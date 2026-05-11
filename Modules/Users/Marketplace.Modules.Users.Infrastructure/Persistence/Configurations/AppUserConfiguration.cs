using Marketplace.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Modules.Users.Infrastructure.Persistence.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdentityUserId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.AvatarUrl).HasMaxLength(500);
        builder.Property(x => x.Bio).HasMaxLength(1000);
        builder.Property(x => x.Status).HasConversion<int>();

        builder.HasIndex(x => x.IdentityUserId).IsUnique();
    }
}
