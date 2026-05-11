using AutoMapper;
using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Domain.Entities;

namespace Marketplace.Modules.Notifications.Application.Notifications.Mappings;

public sealed class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<Notification, NotificationDto>();
    }
}
