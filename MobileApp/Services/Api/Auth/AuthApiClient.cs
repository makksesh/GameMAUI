using System.Text.Json;
using MobileApp.Exceptions;
using MobileApp.Models.Auth;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Auth;

public class AuthApiClient : ApiClientBase, IAuthApiClient
{
    public AuthApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }
    

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var content = Serialize(request);
        var response = await Http.PostAsync("auth/login", content);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            throw new AccountBlockedException(); // ✅ специальное исключение

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(json, JsonOptions);
    }


    public Task<AuthResponse?> RegisterAsync(RegisterRequest request) =>
        PostAsync<AuthResponse>("auth/register", request);

    public Task ChangePasswordAsync(ChangePasswordRequest request) =>
        PostAsync("auth/change-password", request);
}