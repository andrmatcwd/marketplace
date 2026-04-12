using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;

public sealed record CreateSubCategoryCommand(
    int CategoryId,
    string Name,
    string Slug,
    string? Description) : IRequest<Unit>;
