using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetById;

public sealed record GetSubCategoryByIdQuery(int Id) : IRequest<SubCategoryDto>;
