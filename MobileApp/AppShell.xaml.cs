using Microsoft.Maui.Controls;
using MobileApp.Services.Storage;
using MobileApp.Views.Admin;
using MobileApp.Views.Auth;
using MobileApp.Views.Character;
using MobileApp.Views.Support;
using MobileApp.Views.Trade;

namespace MobileApp;

public partial class AppShell : Shell
{
    private readonly ISessionService _session;
    private bool _initialized;

    public AppShell(ISessionService session)
    {
        InitializeComponent();
        _session = session;
        RegisterRoutes();
        Loaded += AppShell_Loaded;
    }

    private async void AppShell_Loaded(object? sender, EventArgs e)
    {
        if (_initialized)
            return;

        _initialized = true;

        await _session.TryRestoreAsync();

        var route = _session.IsAuthenticated
            ? "//MainTabs/CharacterTab/CharacterPage"
            : "//LoginPage";

        await GoToAsync(route);
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute("ChangePasswordPage", typeof(ChangePasswordPage));
        Routing.RegisterRoute("UpgradeStatPage", typeof(UpgradeStatPage));
        Routing.RegisterRoute("CreateTradeLotPage", typeof(CreateTradeLotPage));
        Routing.RegisterRoute("AdminUsersPage", typeof(AdminUsersPage));
        Routing.RegisterRoute(nameof(ModeratorSupportPage), typeof(ModeratorSupportPage));

    }
}