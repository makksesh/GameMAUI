using MobileApp.Models.Auth;

namespace MobileApp.Services.Api.Auth;

public interface IAuthApiClient
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request);
}