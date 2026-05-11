using AutoMapper;
using Marketplace.Modules.Users.Application.Users.Dtos;
using Marketplace.Modules.Users.Domain.Entities;

namespace Marketplace.Modules.Users.Application.Users.Mappings;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<AppUser, UserDto>();
    }
}
