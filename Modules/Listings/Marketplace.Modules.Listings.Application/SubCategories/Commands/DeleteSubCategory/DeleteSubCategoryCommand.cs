using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.DeleteSubCategory;

public sealed record DeleteSubCategoryCommand(int Id) : IRequest<int>;
