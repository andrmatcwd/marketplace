using Marketplace.Modules.Users.Application.Common.Models;
using Marketplace.Modules.Users.Application.Users.Commands.CreateUser;
using Marketplace.Modules.Users.Application.Users.Commands.EditUser;
using Marketplace.Modules.Users.Application.Users.Dtos;
using Marketplace.Modules.Users.Application.Users.Filters;

namespace Marketplace.Modules.Users.Application.Services;

public interface IAppUserService
{
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task<PagedResult<UserDto>> GetByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
    Task EditAsync(EditUserCommand command, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
