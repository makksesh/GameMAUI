using MobileApp.Models.Auth;

namespace MobileApp.Services.Storage;

public interface ISessionService
{
    AuthResponse? CurrentUser { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    bool IsModerator { get; }

    Task SaveAsync(AuthResponse response);
    Task ClearAsync();
    Task TryRestoreAsync();
}