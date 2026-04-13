using System;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryBySlags;

public sealed record GetSubCategoryBySlagsQuery(
    string CitySlag,
    string CategorySlag,
    string SubCategorySlag
) : IRequest<SubCategoryDto>;
