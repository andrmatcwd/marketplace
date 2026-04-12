using Marketplace.Modules.Listings.Application.Listings.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetById;

public sealed record GetListingByIdQuery(int Id)
    : IRequest<ListingDto>;
