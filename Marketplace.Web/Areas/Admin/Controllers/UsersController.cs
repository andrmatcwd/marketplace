using Marketplace.Modules.Users.Application.Users.Commands.EditUser;
using Marketplace.Modules.Users.Application.Users.Filters;
using Marketplace.Modules.Users.Application.Users.Queries.GetUserById;
using Marketplace.Modules.Users.Application.Users.Queries.GetUsersByFilter;
using Marketplace.Modules.Users.Domain.Enums;
using Marketplace.Web.Areas.Admin.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly ISender _sender;
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(ISender sender, UserManager<IdentityUser> userManager)
    {
        _sender = sender;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(
        string? search,
        UserStatus? status,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetUsersByFilterQuery(new UserFilter
        {
            Search = search,
            Status = status,
            Page = page,
            PageSize = 20
        }), cancellationToken);

        var adminIdentityUsers = await _userManager.GetUsersInRoleAsync("Admin");
        var adminIds = adminIdentityUsers.Select(x => x.Id).ToHashSet();

        ViewBag.Statuses = Enum.GetValues<UserStatus>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        return View(new UserIndexVm
        {
            Search = search,
            Status = status,
            TotalCount = result.TotalCount,
            Page = result.Page,
            TotalPages = result.TotalPages,
            Items = result.Items.Select(x => new UserListItemVm
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Email = x.Email,
                Status = x.Status,
                IsAdmin = adminIds.Contains(x.IdentityUserId),
                CreatedAtUtc = x.CreatedAtUtc
            }).ToList()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var user = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
        if (user is null) return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId);
        var isAdmin = identityUser is not null && await _userManager.IsInRoleAsync(identityUser, "Admin");

        return View(new UserEditVm
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Phone = user.Phone,
            Bio = user.Bio,
            Status = user.Status,
            IsAdmin = isAdmin,
            IdentityUserId = user.IdentityUserId,
            IsSelf = user.IdentityUserId == currentUserId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _sender.Send(new GetUserByIdQuery(model.Id), cancellationToken);
        if (user is null) return NotFound();

        await _sender.Send(new EditUserCommand(
            model.Id,
            model.DisplayName,
            model.Email,
            model.Phone,
            user.AvatarUrl,
            model.Bio,
            model.Status
        ), cancellationToken);

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (user.IdentityUserId != currentUserId)
        {
            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId);
            if (identityUser is not null)
            {
                var currentlyAdmin = await _userManager.IsInRoleAsync(identityUser, "Admin");
                if (model.IsAdmin && !currentlyAdmin)
                    await _userManager.AddToRoleAsync(identityUser, "Admin");
                else if (!model.IsAdmin && currentlyAdmin)
                    await _userManager.RemoveFromRoleAsync(identityUser, "Admin");
            }
        }

        TempData["Success"] = $"User \"{user.DisplayName}\" updated.";
        return RedirectToAction(nameof(Index));
    }
}
