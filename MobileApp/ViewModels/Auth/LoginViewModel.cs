using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MobileApp.Models.Auth;
using MobileApp.Services.Api.Auth;
using MobileApp.Services.Storage;

namespace MobileApp.ViewModels.Auth;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthApiClient _authApi;
    private readonly ISessionService _session;

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public Command LoginCommand { get; }
    public Command GoToRegisterCommand { get; }

    public LoginViewModel(IAuthApiClient authApi, ISessionService session)
    {
        _authApi = authApi;
        _session = session;

        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        GoToRegisterCommand = new Command(async () =>
            await Shell.Current.GoToAsync("//RegisterPage"));
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введите логин и пароль";
            return;
        }

        await RunSafeAsync(async () =>
        {
            var result = await _authApi.LoginAsync(new LoginRequest
            {
                Username = Username,
                Password = Password
            });

            if (result is null)
            {
                ErrorMessage = "Неверный логин или пароль";
                return;
            }

            await _session.SaveAsync(result);
            await Shell.Current.GoToAsync("//MainTabs/CharacterTab/CharacterPage");
        });
    }
}