using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace E_learning.Client.Services;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim RefreshSemaphore = new(1, 1);

    private readonly ITokenStorageService _tokenStorageService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthorizationMessageHandler(
        ITokenStorageService tokenStorageService,
        IHttpClientFactory httpClientFactory,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _tokenStorageService = tokenStorageService;
        _httpClientFactory = httpClientFactory;
        _authenticationStateProvider = authenticationStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var expiresAtStr = await _tokenStorageService.GetItemAsync<string>("expiresAt");
        if (!string.IsNullOrWhiteSpace(expiresAtStr) &&
            DateTime.TryParse(expiresAtStr, null, DateTimeStyles.RoundtripKind, out var expiresAt) &&
            expiresAt <= DateTime.UtcNow.AddMinutes(5))
        {
            await RefreshSemaphore.WaitAsync(cancellationToken);
            try
            {
                expiresAtStr = await _tokenStorageService.GetItemAsync<string>("expiresAt");
                if (!string.IsNullOrWhiteSpace(expiresAtStr) &&
                    DateTime.TryParse(expiresAtStr, null, DateTimeStyles.RoundtripKind, out expiresAt) &&
                    expiresAt <= DateTime.UtcNow.AddMinutes(5))
                {
                    await RefreshTokenInternalAsync(cancellationToken);
                }
            }
            finally
            {
                RefreshSemaphore.Release();
            }
        }

        var token = await _tokenStorageService.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task RefreshTokenInternalAsync(CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenStorageService.GetItemAsync<string>("refreshToken");
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var authClient = _httpClientFactory.CreateClient("AuthClient");
        var response = await authClient.PostAsJsonAsync("/refresh", new { refreshToken }, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ClearTokensAsync();
            return;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
        if (loginResponse == null)
        {
            await ClearTokensAsync();
            return;
        }

        await _tokenStorageService.SetItemAsync("authToken", loginResponse.AccessToken);
        await _tokenStorageService.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        await _tokenStorageService.SetItemAsync("expiresAt", DateTime.UtcNow.AddSeconds(loginResponse.ExpiresIn).ToString("o"));

        if (_authenticationStateProvider is CustomAuthenticationStateProvider provider)
        {
            await provider.NotifyUserAuthenticationAsync();
        }
    }

    private async Task ClearTokensAsync()
    {
        await _tokenStorageService.RemoveItemAsync("authToken");
        await _tokenStorageService.RemoveItemAsync("refreshToken");
        await _tokenStorageService.RemoveItemAsync("userEmail");
        await _tokenStorageService.RemoveItemAsync("expiresAt");
        await _tokenStorageService.RemoveItemAsync("userRoles");

        if (_authenticationStateProvider is CustomAuthenticationStateProvider provider)
        {
            provider.NotifyUserLogout();
        }
    }
}