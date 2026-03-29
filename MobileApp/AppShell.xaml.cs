using MobileApp.Services.Storage;
using MobileApp.Views.Auth;
using MobileApp.Views.Character;
using MobileApp.Views.Friends;
using MobileApp.Views.Home;
using MobileApp.Views.Inventory;
using MobileApp.Views.Profile;
using MobileApp.Views.Skills;
using MobileApp.Views.Support;
using MobileApp.Views.Trade;

namespace MobileApp;

public partial class AppShell : Shell
{
    private const string RouteHome    = "//HomePage";
    private const string RouteLogin   = "//LoginPage";
    private const string RouteStub    = "//RoleStubPage";

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
        if (_initialized) return;
        _initialized = true;
        
        await _session.TryRestoreAsync();
        
        if (!_session.IsAuthenticated)
        {
            await GoToAsync(RouteLogin);
            return;
        }

        var route = _session.IsPlayer
            ? RouteHome
            : RouteStub;

        await GoToAsync(route);
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(LoginPage),          typeof(LoginPage));
        Routing.RegisterRoute(nameof(CharacterPage),      typeof(CharacterPage));
        Routing.RegisterRoute(nameof(SupportPage),        typeof(SupportPage));
        Routing.RegisterRoute(nameof(SkillsPage),         typeof(SkillsPage));
        Routing.RegisterRoute(nameof(InventoryPage),      typeof(InventoryPage));
        Routing.RegisterRoute(nameof(TradeLotsPage),      typeof(TradeLotsPage));
        Routing.RegisterRoute(nameof(FriendsPage),        typeof(FriendsPage));
        Routing.RegisterRoute(nameof(ProfilePage),        typeof(ProfilePage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(UpgradeStatPage),    typeof(UpgradeStatPage));
        Routing.RegisterRoute(nameof(CreateTradeLotPage), typeof(CreateTradeLotPage));
    }
}
