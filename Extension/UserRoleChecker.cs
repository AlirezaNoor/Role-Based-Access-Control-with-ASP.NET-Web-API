using System.Security.Claims;
using RoleBasedAuthSample.Data;

namespace RoleBasedAuthSample.Extension;

public static class UserRoleChecker
{
     
    public static bool CheckUserRole(ClaimsPrincipal user,int id)
    {
        // اگر کاربر نامعتبر یا خالی است، false برگردانید
        if (user == null || !user.Identity.IsAuthenticated)
            return false;

        
        // بررسی هر Claim کاربر برای نقش موردنظر
        foreach (var claim in user.Claims)
        {
            if (claim.Type == "accessId" && claim.Value == id.ToString())
                return true;
        }

        // اگر نقش موردنظر پیدا نشد، false برگردانید
        return false;
    }
}