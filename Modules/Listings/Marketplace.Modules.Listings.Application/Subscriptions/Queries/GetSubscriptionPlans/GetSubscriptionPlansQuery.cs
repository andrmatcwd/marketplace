using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlans;

public sealed record GetSubscriptionPlansQuery(bool ActiveOnly = false) : IRequest<IReadOnlyList<SubscriptionPlanDto>>;
