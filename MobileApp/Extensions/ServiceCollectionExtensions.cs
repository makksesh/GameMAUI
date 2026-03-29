using MobileApp.Services.Api.Auth;
using MobileApp.Services.Api.Character;
using MobileApp.Services.Api.Friends;
using MobileApp.Services.Api.Inventory;
using MobileApp.Services.Api.Skills;
using MobileApp.Services.Api.Support;
using MobileApp.Services.Api.Trade;
using MobileApp.Services.Storage;
using MobileApp.ViewModels.Auth;
using MobileApp.ViewModels.Character;
using MobileApp.ViewModels.Friends;
using MobileApp.ViewModels.Home;
using MobileApp.ViewModels.Inventory;
using MobileApp.ViewModels.Profile;
using MobileApp.ViewModels.Skills;
using MobileApp.ViewModels.Support;
using MobileApp.ViewModels.Trade;
using MobileApp.Views.Auth;
using MobileApp.Views.Character;
using MobileApp.Views.Friends;
using MobileApp.Views.Home;
using MobileApp.Views.Inventory;
using MobileApp.Views.Profile;
using MobileApp.Views.Skills;
using MobileApp.Views.Support;
using MobileApp.Views.Trade;

namespace MobileApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("GameRpgApi", client =>
        {
#if __IOS__
            client.BaseAddress = new Uri("https://localhost:7001/api/");
#elif __ANDROID__
            client.BaseAddress = new Uri("http://10.0.2.2:7001/api/");
#else
            client.BaseAddress = new Uri("http://localhost:7001/api/");
#endif
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddSingleton<ITokenStorage, TokenStorage>();
        services.AddSingleton<ISessionService, SessionService>();
        return services;
    }

    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddTransient<IAuthApiClient,      AuthApiClient>();
        services.AddTransient<ICharacterApiClient, CharacterApiClient>();
        services.AddTransient<ISkillsApiClient,    SkillsApiClient>();
        services.AddTransient<IInventoryApiClient, InventoryApiClient>();
        services.AddTransient<ITradeApiClient,     TradeApiClient>();
        services.AddTransient<IFriendsApiClient,   FriendsApiClient>();
        services.AddTransient<ISupportApiClient,   SupportApiClient>();
        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<ChangePasswordViewModel>();

        services.AddTransient<CharacterViewModel>();
        services.AddTransient<UpgradeStatViewModel>();
        services.AddTransient<SkillsViewModel>();
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<TradeViewModel>();
        services.AddTransient<FriendsViewModel>();
        services.AddTransient<SupportViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<TicketChatViewModel>(); 
        services.AddTransient<HomeViewModel>();
        services.AddTransient<BlockedViewModel>();
        return services;
    }

    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        services.AddTransient<LoginPage>();
        services.AddTransient<RegisterPage>();
        services.AddTransient<ChangePasswordPage>();

        services.AddTransient<CharacterPage>();
        services.AddTransient<UpgradeStatPage>();
        services.AddTransient<SkillsPage>();
        services.AddTransient<InventoryPage>();
        services.AddTransient<TradeLotsPage>();
        services.AddTransient<CreateTradeLotPage>();
        services.AddTransient<FriendsPage>();
        services.AddTransient<SupportPage>();
        services.AddTransient<ProfilePage>();
        services.AddTransient<TicketChatPage>();
        services.AddTransient<HomePage>();
        services.AddTransient<RoleStubPage>();
        services.AddTransient<BlockedPage>(); 
        return services;
    }
}