using System.Security.Claims;

namespace Business.Services;

public interface IAuthorizationService
{
    Task<bool> IsUserInRoleAsync(string userId, string roleName);
    Task<bool> IsUserInRoleAsync(ClaimsPrincipal user, string roleName);
    Task<bool> IsUserInRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
}
