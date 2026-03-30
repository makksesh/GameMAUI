using MobileApp.Services.Api.Character;
using MobileApp.Services.Api.Friends;
using MobileApp.Services.Storage;

namespace MobileApp.ViewModels.Home;

public class HomeViewModel : BaseViewModel
{
    private readonly ICharacterApiClient _characterApi;
    private readonly IFriendsApiClient   _friendsApi;
    private readonly ISessionService     _session;

    // ── Деньги персонажа ──────────────────────────────────────────
    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    // ── Уведомление о запросе в друзья ───────────────────────────
    private bool _hasFriendRequest;
    public bool HasFriendRequest
    {
        get => _hasFriendRequest;
        set => SetProperty(ref _hasFriendRequest, value);
    }

    private string _friendRequestSender = string.Empty;
    public string FriendRequestSender
    {
        get => _friendRequestSender;
        set => SetProperty(ref _friendRequestSender, value);
    }

    // ── Команды навигации ─────────────────────────────────────────
    public Command LoadCommand         { get; }
    public Command GoToSkillsCommand   { get; }
    public Command GoToShopCommand     { get; }
    public Command GoToInventoryCommand{ get; }
    public Command GoToProfileCommand  { get; }
    public Command GoToFriendsCommand  { get; }
    public Command DismissNotificationCommand { get; }
    public Command StartAloneCommand { get; }
    public Command StartCoopCommand  { get; }

    public HomeViewModel(
        ICharacterApiClient characterApi,
        IFriendsApiClient   friendsApi,
        ISessionService     session)
    {
        _characterApi = characterApi;
        _friendsApi   = friendsApi;
        _session      = session;

        LoadCommand          = new Command(async () => await LoadAsync());
        GoToSkillsCommand    = new Command(async () => await Shell.Current.GoToAsync("SkillsPage"));
        GoToShopCommand      = new Command(async () => await Shell.Current.GoToAsync("TradeLotsPage"));
        GoToInventoryCommand = new Command(async () => await Shell.Current.GoToAsync("CharacterPage"));
        GoToProfileCommand   = new Command(async () => await Shell.Current.GoToAsync("ProfilePage"));
        GoToFriendsCommand   = new Command(async () => await Shell.Current.GoToAsync("FriendsPage"));

        StartAloneCommand = new Command(async () =>
        {
            await Shell.Current.DisplayAlert("Режим", "Одиночный режим", "OK");
        });

        StartCoopCommand = new Command(async () =>
        {
            await Shell.Current.DisplayAlert("Режим", "Кооператив", "OK");
        });
        DismissNotificationCommand = new Command(() => HasFriendRequest = false);
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            // Загружаем баланс персонажа
            Console.WriteLine("HOME: before GetMyAsync");
            var character = await _characterApi.GetMyAsync();
            Balance = character?.Balance ?? 0;
            Console.WriteLine($"HOME: after GetMyAsync, null = {character is null}");
            // Проверяем входящие запросы в друзья
            Console.WriteLine("HOME: before GetIncomingRequestsAsync");
            var requests = await _friendsApi.GetIncomingRequestsAsync();
            Console.WriteLine($"HOME: after GetIncomingRequestsAsync, count = {requests?.Count ?? 0}");
            var pending  = requests?.FirstOrDefault(r => r.Status == "Pending");

            if (pending is not null)
            {
                HasFriendRequest    = true;
                FriendRequestSender = pending.SenderUsername;
            }
            else
            {
                HasFriendRequest = false;
            }
        });
        Console.WriteLine($"HOME: LoadAsync finish, IsBusy = {IsBusy}, Error = {ErrorMessage}");
    }
}