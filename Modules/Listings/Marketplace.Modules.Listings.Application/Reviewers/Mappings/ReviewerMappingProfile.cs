using AutoMapper;
using Marketplace.Modules.Listings.Application.Reviewers.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Reviewers.Mappings;

public sealed class ReviewerMappingProfile : Profile
{
    public ReviewerMappingProfile()
    {
        CreateMap<Reviewer, ReviewerDto>();
    }
}
