using Marketplace.Modules.Listings.Application.Categories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetById;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
