using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence;

public sealed class ListingsDbContext : DbContext
{
    public ListingsDbContext(DbContextOptions<ListingsDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Listing>(builder =>
        {
            builder.ToTable("Listings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.SellerId)
                .HasMaxLength(450)
                .IsRequired();
        });
    }
}
