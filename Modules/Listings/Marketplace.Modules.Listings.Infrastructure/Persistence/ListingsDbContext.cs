using Marketplace.Modules.Listings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence;

public sealed class ListingsDbContext : DbContext
{
    public ListingsDbContext(DbContextOptions<ListingsDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<ListingRental> ListingRentals => Set<ListingRental>();
    public DbSet<ListingRentalRoom> ListingRentalRooms => Set<ListingRentalRoom>();
    public DbSet<ListingVacancy> ListingVacancies => Set<ListingVacancy>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Reviewer> Reviewers => Set<Reviewer>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<ListingSubscription> ListingSubscriptions => Set<ListingSubscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListingsDbContext).Assembly);
    }
}
