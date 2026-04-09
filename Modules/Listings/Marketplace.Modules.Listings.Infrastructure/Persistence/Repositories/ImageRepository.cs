using System;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Infrastructure.Persistence.Repositories;

public class ImageRepository
    : BaseRepository<Image, int>, IImageRepository
{
    public ImageRepository(ListingsDbContext dbContext) : base(dbContext)
    {
    }

}
