using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository subCategoryRepository;

    public SubCategoryService(ISubCategoryRepository subCategoryRepository)
    {
        this.subCategoryRepository = subCategoryRepository;
    }
}
