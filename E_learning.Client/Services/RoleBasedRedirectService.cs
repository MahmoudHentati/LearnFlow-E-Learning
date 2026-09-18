using E_learning.Models;

namespace E_learning.Client.Services;

public class RoleBasedRedirectService : IRoleBasedRedirectService
{
    public string GetRedirectUrl(string[] userRoles)
    {
        ArgumentNullException.ThrowIfNull(userRoles);

        if (userRoles.Contains(AppRoles.SuperAdmin, StringComparer.OrdinalIgnoreCase) ||
            userRoles.Contains(AppRoles.Admin, StringComparer.OrdinalIgnoreCase))
        {
            return "/";
        }

        if (userRoles.Contains(AppRoles.Instructor, StringComparer.OrdinalIgnoreCase))
        {
            return "/formateur-dashboard";
        }
        if (userRoles.Contains(AppRoles.Student, StringComparer.OrdinalIgnoreCase))
        {
            return "/etudiant-dashboard";
        }

        return "/";
    }
}
