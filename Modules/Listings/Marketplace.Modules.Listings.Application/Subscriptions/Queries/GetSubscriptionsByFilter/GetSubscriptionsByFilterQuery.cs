using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionsByFilter;

public sealed record GetSubscriptionsByFilterQuery(SubscriptionFilter Filter) : IRequest<PagedResult<ListingSubscriptionDto>>;
