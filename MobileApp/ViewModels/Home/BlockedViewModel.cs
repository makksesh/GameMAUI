using MobileApp.Services.Storage;

namespace MobileApp.ViewModels.Home;

public class BlockedViewModel : BaseViewModel
{
    private readonly ISessionService _session;

    public string BlockedUntilText => _session.CurrentUser?.BlockedUntil is DateTime until
        ? $"Блокировка до {until:dd.MM.yyyy HH:mm}"
        : "Аккаунт заблокирован навсегда.";

    public Command LogoutCommand { get; }
    public Command GoToSupportCommand { get; }

    public BlockedViewModel(ISessionService session)
    {
        _session = session;

        LogoutCommand = new Command(async () =>
        {
            await _session.ClearAsync();
            await Shell.Current.GoToAsync("//LoginPage");
        });

        GoToSupportCommand = new Command(async () =>
            await Shell.Current.GoToAsync("SupportPage"));
    }
}