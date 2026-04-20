using Marketplace.Modules.Listings.Application.Listings.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingBySlags;

public sealed record GetListingBySlagsQuery(
    string CitySlag,
    string CategorySlag,
    string SubCategorySlag,
    string ListingSlug,
    int Id
) : IRequest<ListingDto>;
