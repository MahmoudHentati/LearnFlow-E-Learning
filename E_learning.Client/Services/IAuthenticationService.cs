using E_learning.Models.DTOs;

namespace E_learning.Client.Services;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
    Task<bool> RegisterAsync(RegisterRequest registerRequest);
    Task LogoutAsync();
    Task<bool> RefreshTokenAsync();
}