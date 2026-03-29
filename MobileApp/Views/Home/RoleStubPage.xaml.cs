using MobileApp.Services.Storage;

namespace MobileApp.Views.Home;

public partial class RoleStubPage : ContentPage
{
    private readonly ISessionService _session;

    public RoleStubPage(ISessionService session)
    {
        InitializeComponent();
        _session = session;
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await _session.ClearAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}