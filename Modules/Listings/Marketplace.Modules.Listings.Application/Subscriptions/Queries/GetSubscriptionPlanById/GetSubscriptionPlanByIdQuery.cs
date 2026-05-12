using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlanById;

public sealed record GetSubscriptionPlanByIdQuery(int Id) : IRequest<SubscriptionPlanDto?>;
