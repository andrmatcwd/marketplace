using AutoMapper;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Reviews.Mappings;

public sealed class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<Review, ReviewDto>();
    }
}
