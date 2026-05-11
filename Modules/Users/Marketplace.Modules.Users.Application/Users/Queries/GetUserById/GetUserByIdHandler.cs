using Marketplace.Modules.Users.Application.Services;
using Marketplace.Modules.Users.Application.Users.Dtos;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IAppUserService _service;

    public GetUserByIdHandler(IAppUserService service) => _service = service;

    public Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
