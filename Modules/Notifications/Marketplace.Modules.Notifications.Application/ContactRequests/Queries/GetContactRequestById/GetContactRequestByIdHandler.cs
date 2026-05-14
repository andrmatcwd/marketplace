using Marketplace.Modules.Notifications.Application.ContactRequests.Dtos;
using Marketplace.Modules.Notifications.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestById;

public sealed class GetContactRequestByIdHandler : IRequestHandler<GetContactRequestByIdQuery, ContactRequestDto?>
{
    private readonly IContactRequestRepository _repository;

    public GetContactRequestByIdHandler(IContactRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRequestDto?> Handle(GetContactRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return null;

        return new ContactRequestDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Status = entity.Status,
            CustomerName = entity.CustomerName,
            CustomerPhone = entity.CustomerPhone,
            CustomerEmail = entity.CustomerEmail,
            CustomerCompany = entity.CustomerCompany,
            Message = entity.Message,
            ListingId = entity.ListingId,
            ListingTitle = entity.ListingTitle,
            AdminNotes = entity.AdminNotes,
            CreatedAtUtc = entity.CreatedAtUtc,
            ProcessedAtUtc = entity.ProcessedAtUtc
        };
    }
}
