using System.Net.Http.Json;
using E_learning.Models;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace E_learning.Client.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenStorageService _tokenStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(
        IHttpClientFactory httpClientFactory,
        ITokenStorageService tokenStorageService,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenStorageService = tokenStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
    {
        ArgumentNullException.ThrowIfNull(loginRequest);

        await _tokenStorageService.RemoveItemAsync("pendingInstructorApproval");

        var authClient = _httpClientFactory.CreateClient("AuthClient");
        var loginBody = new
        {
            email = loginRequest.Email,
            password = loginRequest.Password
        };

        var response = await authClient.PostAsJsonAsync("/login?useCookies=false&useSessionCookies=false", loginBody);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginResponse == null)
        {
            return null;
        }

        await _tokenStorageService.SetItemAsync("authToken", loginResponse.AccessToken);
        await _tokenStorageService.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        await _tokenStorageService.SetItemAsync("userEmail", loginRequest.Email);
        await _tokenStorageService.SetItemAsync("expiresAt", DateTime.UtcNow.AddSeconds(loginResponse.ExpiresIn).ToString("o"));

        var profile = await FetchAndStoreProfileAsync();

        if (profile is not null
            && profile.Roles.Contains(AppRoles.Instructor, StringComparer.OrdinalIgnoreCase)
            && !profile.IsActive)
        {
            await LogoutAsync();
            await _tokenStorageService.SetItemAsync("pendingInstructorApproval", true);
            return null;
        }

        if (_authenticationStateProvider is CustomAuthenticationStateProvider provider)
        {
            await provider.NotifyUserAuthenticationAsync();
        }

        return loginResponse;
    }

    public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
    {
        ArgumentNullException.ThrowIfNull(registerRequest);

        var authClient = _httpClientFactory.CreateClient("AuthClient");
        var response = await authClient.PostAsJsonAsync("api/Auth/register", registerRequest);

        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _tokenStorageService.RemoveItemAsync("authToken");
        await _tokenStorageService.RemoveItemAsync("refreshToken");
        await _tokenStorageService.RemoveItemAsync("userEmail");
        await _tokenStorageService.RemoveItemAsync("expiresAt");
        await _tokenStorageService.RemoveItemAsync("userRoles");
        await _tokenStorageService.RemoveItemAsync("pendingInstructorApproval");

        if (_authenticationStateProvider is CustomAuthenticationStateProvider provider)
        {
            provider.NotifyUserLogout();
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = await _tokenStorageService.GetItemAsync<string>("refreshToken");
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        var authClient = _httpClientFactory.CreateClient("AuthClient");
        var response = await authClient.PostAsJsonAsync("/refresh", new { refreshToken });

        if (!response.IsSuccessStatusCode)
        {
            await LogoutAsync();
            return false;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginResponse == null)
        {
            await LogoutAsync();
            return false;
        }

        await _tokenStorageService.SetItemAsync("authToken", loginResponse.AccessToken);
        await _tokenStorageService.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        await _tokenStorageService.SetItemAsync("expiresAt", DateTime.UtcNow.AddSeconds(loginResponse.ExpiresIn).ToString("o"));

        if (_authenticationStateProvider is CustomAuthenticationStateProvider provider)
        {
            await provider.NotifyUserAuthenticationAsync();
        }

        return true;
    }

    private async Task<UserProfileDTO?> FetchAndStoreProfileAsync()
    {
        var apiClient = _httpClientFactory.CreateClient("ApiClient");
        var profileResponse = await apiClient.GetAsync("api/Auth/profile");
        if (!profileResponse.IsSuccessStatusCode)
        {
            await _tokenStorageService.SetItemAsync("userRoles", Array.Empty<string>());
            await _tokenStorageService.RemoveItemAsync("userName");
            return null;
        }

        var profile = await profileResponse.Content.ReadFromJsonAsync<UserProfileDTO>();
        if (profile != null)
        {
            await _tokenStorageService.SetItemAsync("userRoles", profile.Roles ?? Array.Empty<string>());
            
            var fullName = $"{profile.FirstName} {profile.LastName}".Trim();
            if (string.IsNullOrEmpty(fullName)) fullName = profile.Email;
            
            await _tokenStorageService.SetItemAsync("userName", fullName);
            await _tokenStorageService.SetItemAsync("userEmail", profile.Email);
            return profile;
        }

        await _tokenStorageService.SetItemAsync("userRoles", Array.Empty<string>());
        return null;
    }
}
