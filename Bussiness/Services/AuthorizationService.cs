using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Services;

public class AuthorizationService(UserManager<UserEntity> userManager) : IAuthorizationService
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<bool> IsUserInRoleAsync(ClaimsPrincipal userPrincipal, string roleName)
    {
        var user = await _userManager.GetUserAsync(userPrincipal);
        return user != null && await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, roleName);
    }
}
