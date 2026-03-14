using MobileApp.Models.Auth;
using MobileApp.Services.Api.Auth;

namespace MobileApp.ViewModels.Auth;

public class ChangePasswordViewModel : BaseViewModel
{
    private readonly IAuthApiClient _authApi;

    public string CurrentPassword  { get => _cur;  set => SetProperty(ref _cur,  value); }
    public string NewPassword      { get => _new;  set => SetProperty(ref _new,  value); }
    public string ConfirmPassword  { get => _conf; set => SetProperty(ref _conf, value); }

    private string _cur  = string.Empty;
    private string _new  = string.Empty;
    private string _conf = string.Empty;

    public Command SaveCommand { get; }
    public Command CancelCommand { get; }

    public ChangePasswordViewModel(IAuthApiClient authApi)
    {
        _authApi = authApi;
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsBusy);
        CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task SaveAsync()
    {
        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "Новые пароли не совпадают";
            return;
        }

        await RunSafeAsync(async () =>
        {
            await _authApi.ChangePasswordAsync(new ChangePasswordRequest
            {
                CurrentPassword    = CurrentPassword,
                NewPassword        = NewPassword,
                ConfirmNewPassword = ConfirmPassword
            });

            await Shell.Current.DisplayAlert("Успех", "Пароль изменён", "OK");
            await Shell.Current.GoToAsync("..");
        });
    }
}