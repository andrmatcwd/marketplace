using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Filters;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Services.Implementations;

public class ListingSubscriptionService : IListingSubscriptionService
{
    private readonly IListingSubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly IListingRepository _listingRepository;

    public ListingSubscriptionService(
        IListingSubscriptionRepository subscriptionRepository,
        ISubscriptionPlanRepository planRepository,
        IListingRepository listingRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<ListingSubscriptionDto>> GetByFilterAsync(SubscriptionFilter filter, CancellationToken cancellationToken)
    {
        var (items, total) = await _subscriptionRepository.GetByFilterAsync(filter, cancellationToken);
        return new PagedResult<ListingSubscriptionDto>
        {
            Items = items.Select(ToDto).ToList(),
            TotalCount = total
        };
    }

    public async Task<IReadOnlyList<ListingSubscriptionDto>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken)
    {
        var items = await _subscriptionRepository.GetByListingIdAsync(listingId, cancellationToken);
        return items.Select(ToDto).ToList();
    }

    public async Task AssignAsync(int listingId, int planId, DateTime startsAt, string? assignedByUserId, string? notes, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken)
            ?? throw new Exception($"Listing {listingId} not found.");

        var plan = await _planRepository.GetByIdAsync(planId, cancellationToken)
            ?? throw new Exception($"SubscriptionPlan {planId} not found.");

        var expiresAt = startsAt.AddDays(plan.DurationDays);

        var subscription = new ListingSubscription
        {
            ListingId = listingId,
            PlanId = planId,
            AssignedByUserId = assignedByUserId,
            StartsAt = startsAt,
            ExpiresAt = expiresAt,
            Status = SubscriptionStatus.Active,
            Notes = notes,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _subscriptionRepository.AddAsync(subscription, cancellationToken);

        listing.SubscriptionType = plan.SubscriptionType;
        listing.SubscriptionExpiresAt = expiresAt;
        _listingRepository.Update(listing);

        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAsync(int subscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, cancellationToken)
            ?? throw new Exception($"ListingSubscription {subscriptionId} not found.");

        subscription.Status = SubscriptionStatus.Cancelled;
        _subscriptionRepository.Update(subscription);

        var listing = await _listingRepository.GetByIdAsync(subscription.ListingId, cancellationToken);
        if (listing is not null && listing.SubscriptionExpiresAt == subscription.ExpiresAt)
        {
            listing.SubscriptionType = SubscriptionType.Free;
            listing.SubscriptionExpiresAt = null;
            _listingRepository.Update(listing);
        }

        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task ExpireOverdueAsync(CancellationToken cancellationToken)
    {
        var expired = await _subscriptionRepository.GetExpiredActiveAsync(DateTime.UtcNow, cancellationToken);

        foreach (var subscription in expired)
        {
            subscription.Status = SubscriptionStatus.Expired;
            _subscriptionRepository.Update(subscription);

            var listing = await _listingRepository.GetByIdAsync(subscription.ListingId, cancellationToken);
            if (listing is not null && listing.SubscriptionExpiresAt <= DateTime.UtcNow)
            {
                listing.SubscriptionType = SubscriptionType.Free;
                listing.SubscriptionExpiresAt = null;
                _listingRepository.Update(listing);
            }
        }

        if (expired.Count > 0)
            await _subscriptionRepository.SaveChangesAsync(cancellationToken);
    }

    private static ListingSubscriptionDto ToDto(ListingSubscription s) => new()
    {
        Id = s.Id,
        ListingId = s.ListingId,
        ListingTitle = s.Listing?.Title ?? string.Empty,
        PlanId = s.PlanId,
        PlanName = s.Plan?.Name ?? string.Empty,
        SubscriptionType = s.Plan?.SubscriptionType ?? SubscriptionType.Free,
        AssignedByUserId = s.AssignedByUserId,
        RequestedByUserId = s.RequestedByUserId,
        StartsAt = s.StartsAt,
        ExpiresAt = s.ExpiresAt,
        Status = s.Status,
        Notes = s.Notes,
        CreatedAtUtc = s.CreatedAtUtc
    };
}
