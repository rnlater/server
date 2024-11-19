using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Utils
{
    public class UserExtractor
    {

        public static Guid? GetUserId(IHttpContextAccessor _httpContextAccessor)
        {
            var userIdClaimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaimValue))
                return null;

            return Guid.TryParse(userIdClaimValue, out var userId) ? userId : (Guid?)null;
        }

        public static string? GetUserRole(IHttpContextAccessor _httpContextAccessor)
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value;
        }

        public static ClaimsPrincipal? GetUser(IHttpContextAccessor _httpContextAccessor)
        {
            return _httpContextAccessor.HttpContext?.User;
        }
    }
}