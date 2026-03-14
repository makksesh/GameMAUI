namespace MobileApp.Services.Storage;

public interface ITokenStorage
{
    Task SaveAsync(string token);
    Task<string?> GetAsync();
    Task ClearAsync();
}