using Marketplace.Modules.Users.Application.Common.Models;
using Marketplace.Modules.Users.Application.Services;
using Marketplace.Modules.Users.Application.Users.Dtos;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Queries.GetUsersByFilter;

public sealed class GetUsersByFilterHandler : IRequestHandler<GetUsersByFilterQuery, PagedResult<UserDto>>
{
    private readonly IAppUserService _service;

    public GetUsersByFilterHandler(IAppUserService service) => _service = service;

    public Task<PagedResult<UserDto>> Handle(GetUsersByFilterQuery request, CancellationToken cancellationToken)
        => _service.GetByFilterAsync(request.Filter, cancellationToken);
}
