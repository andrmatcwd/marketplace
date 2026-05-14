using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.ContactRequests.Dtos;
using Marketplace.Modules.Notifications.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestsByFilter;

public sealed class GetContactRequestsByFilterHandler
    : IRequestHandler<GetContactRequestsByFilterQuery, PagedResult<ContactRequestDto>>
{
    private readonly IContactRequestRepository _repository;

    public GetContactRequestsByFilterHandler(IContactRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ContactRequestDto>> Handle(
        GetContactRequestsByFilterQuery request,
        CancellationToken cancellationToken)
    {
        var (items, total) = await _repository.GetByFilterAsync(request.Filter, cancellationToken);

        return new PagedResult<ContactRequestDto>
        {
            Items = items.Select(x => new ContactRequestDto
            {
                Id = x.Id,
                Type = x.Type,
                Status = x.Status,
                CustomerName = x.CustomerName,
                CustomerPhone = x.CustomerPhone,
                CustomerEmail = x.CustomerEmail,
                CustomerCompany = x.CustomerCompany,
                Message = x.Message,
                ListingId = x.ListingId,
                ListingTitle = x.ListingTitle,
                AdminNotes = x.AdminNotes,
                CreatedAtUtc = x.CreatedAtUtc,
                ProcessedAtUtc = x.ProcessedAtUtc
            }).ToList(),
            Page = request.Filter.Page,
            PageSize = request.Filter.PageSize,
            TotalCount = total
        };
    }
}
