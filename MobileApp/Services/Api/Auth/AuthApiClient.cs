using MobileApp.Models.Auth;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Auth;

public class AuthApiClient : ApiClientBase, IAuthApiClient
{
    public AuthApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<AuthResponse?> LoginAsync(LoginRequest request) =>
        PostAsync<AuthResponse>("auth/login", request);

    public Task<AuthResponse?> RegisterAsync(RegisterRequest request) =>
        PostAsync<AuthResponse>("auth/register", request);

    public Task ChangePasswordAsync(ChangePasswordRequest request) =>
        PostAsync("auth/change-password", request);
}