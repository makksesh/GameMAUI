namespace MobileApp.Services.Storage;

public class TokenStorage : ITokenStorage
{
    private const string Key = "auth_jwt_token";

    public Task SaveAsync(string token) =>
        SecureStorage.SetAsync(Key, token);

    public async Task<string?> GetAsync() =>
        await SecureStorage.GetAsync(Key);

    public Task ClearAsync() =>
        SecureStorage.SetAsync(Key, string.Empty);
}