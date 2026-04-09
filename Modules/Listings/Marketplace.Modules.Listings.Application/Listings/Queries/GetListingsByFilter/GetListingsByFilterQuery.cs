using System;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;

public sealed record GetListingsByFilterQuery(Guid Id) : IRequest<Guid>;
