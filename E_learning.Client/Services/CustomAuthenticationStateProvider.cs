using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace E_learning.Client.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenStorageService _tokenStorageService;

    public CustomAuthenticationStateProvider(ITokenStorageService tokenStorageService)
    {
        _tokenStorageService = tokenStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _tokenStorageService.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                return CreateAnonymousState();
            }

            var expiresAtStr = await _tokenStorageService.GetItemAsync<string>("expiresAt");
            if (!string.IsNullOrWhiteSpace(expiresAtStr) &&
                DateTime.TryParse(expiresAtStr, null, DateTimeStyles.RoundtripKind, out var expiresAt) &&
                expiresAt <= DateTime.UtcNow)
            {
                await ClearAuthStorageAsync();
                return CreateAnonymousState();
            }

            var email = await _tokenStorageService.GetItemAsync<string>("userEmail") ?? string.Empty;
            var userName = await _tokenStorageService.GetItemAsync<string>("userName") ?? string.Empty;
            var roles = await _tokenStorageService.GetItemAsync<string[]>("userRoles") ?? Array.Empty<string>();

            var claims = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                claims.Add(new Claim(ClaimTypes.Name, userName));
            }
            else if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.Name, email));
            }
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }

            foreach (var role in roles.Where(r => !string.IsNullOrWhiteSpace(r)))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return CreateAnonymousState();
        }
    }

    public async Task NotifyUserAuthenticationAsync()
    {
        var state = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(CreateAnonymousState()));
    }

    private static AuthenticationState CreateAnonymousState()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private async Task ClearAuthStorageAsync()
    {
        await _tokenStorageService.RemoveItemAsync("authToken");
        await _tokenStorageService.RemoveItemAsync("refreshToken");
        await _tokenStorageService.RemoveItemAsync("userEmail");
        await _tokenStorageService.RemoveItemAsync("userName");
        await _tokenStorageService.RemoveItemAsync("expiresAt");
        await _tokenStorageService.RemoveItemAsync("userRoles");
    }
}