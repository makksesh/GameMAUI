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
        Console.WriteLine("LOGIN: start");
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
            Console.WriteLine($"LOGIN: after api, result null = {result is null}");
            if (result is null)
            {
                ErrorMessage = "Неверный логин или пароль";
                return;
            }
            Console.WriteLine("LOGIN: before save session");
            await _session.SaveAsync(result);
            Console.WriteLine("LOGIN: before navigation");
            var route = _session.IsPlayer
                ? "//HomePage"
                : "//RoleStubPage";
            await Shell.Current.GoToAsync(route);
            Console.WriteLine("LOGIN: after navigation");
        });
        Console.WriteLine($"LOGIN: finish, IsBusy = {IsBusy}, Error = {ErrorMessage}");
    }
}