namespace E_learning.Client.Services;

public interface IRoleBasedRedirectService
{
    string GetRedirectUrl(string[] userRoles);
}