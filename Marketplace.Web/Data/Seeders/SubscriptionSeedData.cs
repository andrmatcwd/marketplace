using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Data.Seeders;

public static class SubscriptionSeedData
{
    public static List<SubscriptionPlan> BuildPlans() =>
    [
        new()
        {
            Name = "Basic Monthly",
            Description = "Boost visibility in search results for 30 days.",
            SubscriptionType = SubscriptionType.Basic,
            PriceUah = 299,
            DurationDays = 30,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = DateTime.UtcNow
        },
        new()
        {
            Name = "Basic Quarterly",
            Description = "Boost visibility in search results for 90 days.",
            SubscriptionType = SubscriptionType.Basic,
            PriceUah = 749,
            DurationDays = 90,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = DateTime.UtcNow
        },
        new()
        {
            Name = "Premium Monthly",
            Description = "Priority placement, highlighted card, and badge for 30 days.",
            SubscriptionType = SubscriptionType.Premium,
            PriceUah = 699,
            DurationDays = 30,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAtUtc = DateTime.UtcNow
        },
        new()
        {
            Name = "Premium Quarterly",
            Description = "Priority placement, highlighted card, and badge for 90 days.",
            SubscriptionType = SubscriptionType.Premium,
            PriceUah = 1799,
            DurationDays = 90,
            IsActive = true,
            DisplayOrder = 4,
            CreatedAtUtc = DateTime.UtcNow
        },
        new()
        {
            Name = "Featured Monthly",
            Description = "Top placement on all category and city pages for 30 days.",
            SubscriptionType = SubscriptionType.Featured,
            PriceUah = 1299,
            DurationDays = 30,
            IsActive = true,
            DisplayOrder = 5,
            CreatedAtUtc = DateTime.UtcNow
        },
    ];

    public static List<ListingSubscription> BuildSubscriptions(
        List<Listing> listings,
        List<SubscriptionPlan> plans,
        Random random)
    {
        var subscriptions = new List<ListingSubscription>();

        SubscriptionPlan Plan(SubscriptionType type) =>
            plans.Where(p => p.SubscriptionType == type)
                 .OrderBy(_ => random.Next())
                 .First();

        var now = DateTime.UtcNow;

        // Featured — 5 listings
        foreach (var listing in listings.Take(5))
        {
            var plan = Plan(SubscriptionType.Featured);
            var starts = now.AddDays(-random.Next(1, 20));
            var expires = starts.AddDays(plan.DurationDays);

            listing.SubscriptionType = SubscriptionType.Featured;
            listing.SubscriptionExpiresAt = expires;

            subscriptions.Add(new ListingSubscription
            {
                Listing = listing,
                Plan = plan,
                StartsAt = starts,
                ExpiresAt = expires,
                Status = SubscriptionStatus.Active,
                Notes = "Seeded",
                CreatedAtUtc = starts
            });
        }

        // Premium — 15 active listings
        foreach (var listing in listings.Skip(5).Take(15))
        {
            var plan = Plan(SubscriptionType.Premium);
            var starts = now.AddDays(-random.Next(1, 60));
            var expires = starts.AddDays(plan.DurationDays);

            listing.SubscriptionType = SubscriptionType.Premium;
            listing.SubscriptionExpiresAt = expires;

            subscriptions.Add(new ListingSubscription
            {
                Listing = listing,
                Plan = plan,
                StartsAt = starts,
                ExpiresAt = expires,
                Status = SubscriptionStatus.Active,
                Notes = "Seeded",
                CreatedAtUtc = starts
            });
        }

        // Basic — 30 active listings
        foreach (var listing in listings.Skip(20).Take(30))
        {
            var plan = Plan(SubscriptionType.Basic);
            var starts = now.AddDays(-random.Next(1, 80));
            var expires = starts.AddDays(plan.DurationDays);
            var isExpired = expires < now;

            listing.SubscriptionType = isExpired ? SubscriptionType.Free : SubscriptionType.Basic;
            listing.SubscriptionExpiresAt = isExpired ? null : expires;

            subscriptions.Add(new ListingSubscription
            {
                Listing = listing,
                Plan = plan,
                StartsAt = starts,
                ExpiresAt = expires,
                Status = isExpired ? SubscriptionStatus.Expired : SubscriptionStatus.Active,
                Notes = "Seeded",
                CreatedAtUtc = starts
            });
        }

        // Historical (cancelled) — 5 listings that previously had Premium
        foreach (var listing in listings.Skip(50).Take(5))
        {
            var plan = Plan(SubscriptionType.Premium);
            var starts = now.AddDays(-120);
            var expires = starts.AddDays(plan.DurationDays);

            subscriptions.Add(new ListingSubscription
            {
                Listing = listing,
                Plan = plan,
                StartsAt = starts,
                ExpiresAt = expires,
                Status = SubscriptionStatus.Cancelled,
                Notes = "Cancelled seed",
                CreatedAtUtc = starts
            });
        }

        return subscriptions;
    }
}
