using Marketplace.Web.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Data;

public sealed class AdminSeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public AdminSeeder(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task SeedAsync()
    {
        foreach (var role in AppRoles.All)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }

        var email = _config["AdminUser:Email"] ?? "admin@marketplace.com";
        var password = _config["AdminUser:Password"] ?? "Admin@12345";

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (!await _userManager.IsInRoleAsync(user, AppRoles.Admin))
            await _userManager.AddToRoleAsync(user, AppRoles.Admin);
    }
}
