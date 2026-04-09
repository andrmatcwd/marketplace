using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;

public sealed record GetSubCategoriesByFilterQuery(SubCategoryFilter Filter) : IRequest<PagedResult<SubCategoryDto>>;
