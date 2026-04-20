using Marketplace.Web.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<ListingImage> ListingImages => Set<ListingImage>();
    public DbSet<ListingReview> ListingReviews => Set<ListingReview>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(140)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.HasIndex(x => x.Slug)
                .IsUnique();
        });

        builder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(140)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasOne(x => x.Category)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<City>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(140)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.HasIndex(x => x.Slug)
                .IsUnique();
        });

        builder.Entity<Listing>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(220)
                .IsRequired();

            entity.Property(x => x.ShortDescription)
                .HasMaxLength(500);

            entity.Property(x => x.Description);

            entity.Property(x => x.Address)
                .HasMaxLength(300);

            entity.Property(x => x.Phone)
                .HasMaxLength(50);

            entity.Property(x => x.Email)
                .HasMaxLength(120);

            entity.Property(x => x.Website)
                .HasMaxLength(300);

            entity.Property(x => x.Rating)
                .HasPrecision(3, 2);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.Property(x => x.Latitude);
            entity.Property(x => x.Longitude);

            entity.HasIndex(x => x.IsPublished);

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.SubCategory)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.City)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ListingImage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Url)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Alt)
                .HasMaxLength(250);

            entity.HasOne(x => x.Listing)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ListingReview>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.AuthorName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Text)
                .HasMaxLength(2000);

            entity.HasOne(x => x.Listing)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}