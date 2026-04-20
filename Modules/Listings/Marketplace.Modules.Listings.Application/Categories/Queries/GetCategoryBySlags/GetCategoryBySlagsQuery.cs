
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryBySlags;

public sealed record GetCategoryBySlagsQuery(
    string CitySlag,
    string CategorySlag
) : IRequest<CategoryDto>;
