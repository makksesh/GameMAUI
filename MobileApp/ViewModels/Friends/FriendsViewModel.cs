using System;
using MobileApp.Models.Friends;
using MobileApp.Services.Api.Friends;

namespace MobileApp.ViewModels.Friends;

public class FriendsViewModel : BaseViewModel
{
    private readonly IFriendsApiClient _friendsApi;

    public List<FriendshipDto> Friends { get => _friends; set => SetProperty(ref _friends, value); }
    private List<FriendshipDto> _friends = [];

    public Command LoadCommand { get; }
    public Command SendRequestCommand { get; }
    public Command<Guid> AcceptCommand { get; }
    public Command<Guid> DeclineCommand { get; }

    public FriendsViewModel(IFriendsApiClient friendsApi)
    {
        _friendsApi = friendsApi;

        LoadCommand       = new Command(async () => await LoadAsync());
        SendRequestCommand = new Command(async () => await SendRequestAsync());
        AcceptCommand     = new Command<Guid>(async id => await AcceptAsync(id));
        DeclineCommand    = new Command<Guid>(async id => await DeclineAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            var result = await _friendsApi.GetFriendsAsync();
            Console.WriteLine($"Friends count = {result?.Count ?? 0}");
            Friends = result ?? [];
            IncomingRequests = (await _friendsApi.GetIncomingRequestsAsync()) ?? [];
        });

        Console.WriteLine($"Error = {ErrorMessage}");
    }
    
    public List<FriendRequestDto> IncomingRequests
    {
        get => _incomingRequests;
        set => SetProperty(ref _incomingRequests, value);
    }
    private List<FriendRequestDto> _incomingRequests = [];


    private async Task SendRequestAsync()
    {
        var input = await Shell.Current.DisplayPromptAsync(
            "Добавить друга", "Введите никнейм пользователя");

        if (string.IsNullOrEmpty(input)) return;
        var receiverId = input;

        await RunSafeAsync(async () =>
        {
            await _friendsApi.SendRequestAsync(receiverId);
            await Shell.Current.DisplayAlert("Отправлено", "Запрос в друзья отправлен", "OK");
        });
    }

    private async Task AcceptAsync(Guid requestId)
    {
        await RunSafeAsync(async () =>
        {
            await _friendsApi.AcceptRequestAsync(requestId);
            await LoadAsync();
        });
    }

    private async Task DeclineAsync(Guid requestId)
    {
        await RunSafeAsync(async () =>
        {
            await _friendsApi.DeclineRequestAsync(requestId);
        });
    }
}