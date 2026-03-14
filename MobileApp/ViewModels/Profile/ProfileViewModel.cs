using MobileApp.Services.Storage;
using MobileApp.Views.Support;

namespace MobileApp.ViewModels.Profile;

public class ProfileViewModel : BaseViewModel
{
    private readonly ISessionService _session;

    public string Username => _session.CurrentUser?.Username ?? string.Empty;
    public string Email    => _session.CurrentUser?.Email    ?? string.Empty;
    public string Role     => _session.CurrentUser?.Role     ?? string.Empty;
    public bool IsAdmin    => _session.IsAdmin;
    public bool IsModerator => 
        _session.CurrentUser?.Role is "Moderator" or "Admin";

    public Command LogoutCommand { get; }
    public Command GoToChangePasswordCommand { get; }
    public Command GoToAdminCommand { get; }
    public Command GoToModeratorSupportCommand { get; }

    public ProfileViewModel(ISessionService session)
    {
        _session = session;

        LogoutCommand = new Command(async () =>
        {
            await _session.ClearAsync();
            await Shell.Current.GoToAsync("//LoginPage");
        });

        GoToChangePasswordCommand = new Command(async () =>
            await Shell.Current.GoToAsync("ChangePasswordPage"));

        GoToAdminCommand = new Command(async () =>
                await Shell.Current.GoToAsync("AdminUsersPage"),
            () => IsAdmin);
        
        GoToModeratorSupportCommand = new Command(
            async () => await Shell.Current.GoToAsync(nameof(ModeratorSupportPage)),
            () => IsModerator
        );
    }
}