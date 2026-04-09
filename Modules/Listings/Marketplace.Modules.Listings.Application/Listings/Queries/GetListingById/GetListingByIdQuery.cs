using System;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetById;

public sealed record GetListingByIdQuery(Guid Id) : IRequest<Guid>;
