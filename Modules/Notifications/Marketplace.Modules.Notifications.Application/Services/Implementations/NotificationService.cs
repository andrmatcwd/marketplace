using AutoMapper;
using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.Notifications.Commands.CreateNotification;
using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Application.Notifications.Filters;
using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Domain.Entities;

namespace Marketplace.Modules.Notifications.Application.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NotificationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _repository.GetByIdAsync(id, cancellationToken);
        return notification is null ? null : _mapper.Map<NotificationDto>(notification);
    }

    public async Task<PagedResult<NotificationDto>> GetByFilterAsync(NotificationFilter filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetByFilterAsync(filter, cancellationToken);
        return new PagedResult<NotificationDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<NotificationDto>>(items),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<IReadOnlyList<NotificationDto>> GetByRecipientIdAsync(string recipientId, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetByRecipientIdAsync(recipientId, cancellationToken);
        return _mapper.Map<IReadOnlyList<NotificationDto>>(items);
    }

    public Task<int> GetUnreadCountAsync(string recipientId, CancellationToken cancellationToken = default)
        => _repository.GetUnreadCountAsync(recipientId, cancellationToken);

    public async Task AddAsync(CreateNotificationCommand command, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            RecipientId = command.RecipientId,
            Title = command.Title,
            Body = command.Body,
            Url = command.Url,
            Type = command.Type
        };

        await _repository.AddAsync(notification, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception($"Notification with id {id} not found.");

        notification.IsRead = true;
        _repository.Update(notification);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAllAsReadAsync(string recipientId, CancellationToken cancellationToken = default)
    {
        await _repository.MarkAllAsReadAsync(recipientId, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception($"Notification with id {id} not found.");

        _repository.Remove(notification);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
