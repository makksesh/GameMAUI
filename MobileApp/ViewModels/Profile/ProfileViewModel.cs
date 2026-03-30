using MobileApp.Services.Storage;
using MobileApp.Views.Support;

namespace MobileApp.ViewModels.Profile;

public class ProfileViewModel : BaseViewModel
{
    private readonly ISessionService _session;

    // ── Данные пользователя ──────────────────────────────────────────────
    public string Username => _session.CurrentUser?.Username ?? string.Empty;
    public string Email    => _session.CurrentUser?.Email    ?? string.Empty;

    // ── Слайдеры громкости (пока декоративные) ───────────────────────────
    private double _musicVolume = 0.3;
    public double MusicVolume
    {
        get => _musicVolume;
        set => SetProperty(ref _musicVolume, value);
    }

    private double _sfxVolume = 0.7;
    public double SfxVolume
    {
        get => _sfxVolume;
        set => SetProperty(ref _sfxVolume, value);
    }

    // ── Место сохранения ─────────────────────────────────────────────────
    private string _selectedStorage = "Local"; // "Local" | "Cloud"
    public string SelectedStorage
    {
        get => _selectedStorage;
        set
        {
            if (SetProperty(ref _selectedStorage, value))
            {
                OnPropertyChanged(nameof(LocalButtonOpacity));
                OnPropertyChanged(nameof(CloudButtonOpacity));
            }
        }
    }

    public double LocalButtonOpacity => SelectedStorage == "Local" ? 1.0 : 0.5;
    public double CloudButtonOpacity => SelectedStorage == "Cloud" ? 1.0 : 0.5;

    // ── Уведомления (пока декоративные) ──────────────────────────────────
    private bool _notificationsEnabled = true;
    public bool NotificationsEnabled
    {
        get => _notificationsEnabled;
        set => SetProperty(ref _notificationsEnabled, value);
    }

    // ── Команды ──────────────────────────────────────────────────────────
    public Command LogoutCommand              { get; }
    public Command GoToChangePasswordCommand  { get; }
    public Command GoToSupportCommand         { get; }
    public Command AboutUsCommand             { get; }
    public Command<string> SelectStorageCommand { get; }

    public ProfileViewModel(ISessionService session)
    {
        _session = session;

        LogoutCommand = new Command(async () =>
        {
            await _session.ClearAsync();
            await Shell.Current.GoToAsync("LoginPage");
        });

        GoToChangePasswordCommand = new Command(async () =>
            await Shell.Current.GoToAsync("ChangePasswordPage"));

        GoToSupportCommand = new Command(async () =>
            await Shell.Current.GoToAsync("SupportPage"));

        AboutUsCommand = new Command(() => { /* пока ничего */ });

        SelectStorageCommand = new Command<string>(location =>
            SelectedStorage = location);
    }
}