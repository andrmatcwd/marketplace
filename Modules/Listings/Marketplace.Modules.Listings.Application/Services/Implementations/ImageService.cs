using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ImageService : IImageService
{
    private readonly IImageRepository imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }
}
