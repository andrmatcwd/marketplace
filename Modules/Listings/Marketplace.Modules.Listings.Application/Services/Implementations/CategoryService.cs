using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }
}
