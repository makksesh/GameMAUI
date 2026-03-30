using MobileApp.Models.Auth;
using MobileApp.Services.Api.Auth;
using MobileApp.Services.Storage;

namespace MobileApp.ViewModels.Auth;

public class RegisterViewModel : BaseViewModel
{
    private readonly IAuthApiClient _authApi;
    private readonly ISessionService _session;

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _confirmPassword = string.Empty;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public Command RegisterCommand { get; }
    public Command GoToLoginCommand { get; }

    public RegisterViewModel(IAuthApiClient authApi, ISessionService session)
    {
        _authApi = authApi;
        _session = session;

        RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);
        GoToLoginCommand = new Command(async () =>
            await Shell.Current.GoToAsync("//LoginPage"));
    }

    private async Task RegisterAsync()
    {
        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Пароли не совпадают";
            return;
        }

        await RunSafeAsync(async () =>
        {
            var result = await _authApi.RegisterAsync(new RegisterRequest
            {
                Username        = Username,
                Email           = Email,
                Password        = Password,
                ConfirmPassword = ConfirmPassword
            });

            if (result is null) return;

            await _session.SaveAsync(result);
            await Shell.Current.GoToAsync("//HomePage");
        });
    }
}