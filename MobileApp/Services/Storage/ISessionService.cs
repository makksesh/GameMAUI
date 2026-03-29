using MobileApp.Models.Auth;

namespace MobileApp.Services.Storage;

public interface ISessionService
{
    AuthResponse? CurrentUser { get; }
    bool IsAuthenticated { get; }
    bool IsPlayer { get; }

    Task SaveAsync(AuthResponse response);
    Task ClearAsync();
    Task TryRestoreAsync();
}