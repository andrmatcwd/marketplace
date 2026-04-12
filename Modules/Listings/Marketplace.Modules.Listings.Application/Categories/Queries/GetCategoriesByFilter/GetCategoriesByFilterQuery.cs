using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;

public sealed record GetCategoriesByFilterQuery(CategoryFilter Filter)
    : IRequest<PagedResult<CategoryDto>>;
