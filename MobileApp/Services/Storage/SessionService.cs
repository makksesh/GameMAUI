using System;
using MobileApp.Constants;
using MobileApp.Models.Auth;

namespace MobileApp.Services.Storage;

public class SessionService : ISessionService
{
    private readonly ITokenStorage _tokenStorage;
    private const string UserIdKey   = "session_user_id";
    private const string UsernameKey = "session_username";
    private const string EmailKey    = "session_email";
    private const string RoleKey     = "session_role";
    
    private const string SessionVersionKey = "session_version";
    private const string CurrentVersion    = "0";
    
    
    public AuthResponse? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser is not null;
    public bool IsPlayer        => CurrentUser?.Role == Roles.Player; 

    public SessionService(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public async Task SaveAsync(AuthResponse response)
    {
        CurrentUser = response;
        await _tokenStorage.SaveAsync(response.Token);
        await SecureStorage.SetAsync(SessionVersionKey, CurrentVersion);
        
        await SecureStorage.SetAsync(UserIdKey,   response.UserId.ToString());
        await SecureStorage.SetAsync(UsernameKey, response.Username);
        await SecureStorage.SetAsync(EmailKey,    response.Email);
        await SecureStorage.SetAsync(RoleKey,     response.Role);
    }

    public async Task ClearAsync()
    {
        CurrentUser = null;
        await _tokenStorage.ClearAsync();
        SecureStorage.Remove(UserIdKey);
        SecureStorage.Remove(UsernameKey);
        SecureStorage.Remove(EmailKey);
        SecureStorage.Remove(RoleKey);
    }


    public async Task TryRestoreAsync()
    {
        var token    = await _tokenStorage.GetAsync();
        var userId   = await SecureStorage.GetAsync(UserIdKey);
        var username = await SecureStorage.GetAsync(UsernameKey);
        var email    = await SecureStorage.GetAsync(EmailKey);
        var role     = await SecureStorage.GetAsync(RoleKey);
        
        if (!string.IsNullOrWhiteSpace(token) ||
            !string.IsNullOrWhiteSpace(role)  ||
            Guid.TryParse(userId, out var id))
        {
            CurrentUser = null;
            return;
        }
        
        CurrentUser = new AuthResponse
        {
            UserId      = id,
            Username    = username ?? string.Empty,
            Email       = email    ?? string.Empty,
            Role        = role,
            Token       = token
        };
    }
}