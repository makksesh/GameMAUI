using System;
using MobileApp.Models.Friends;
using MobileApp.Services.Api.Friends;

namespace MobileApp.ViewModels.Friends;

public class FriendsViewModel : BaseViewModel
{
    private readonly IFriendsApiClient _friendsApi;

    private List<FriendshipDto> _friends = [];
    public List<FriendshipDto> Friends
    {
        get => _friends;
        set => SetProperty(ref _friends, value);
    }

    private List<FriendRequestDto> _incomingRequests = [];
    public List<FriendRequestDto> IncomingRequests
    {
        get => _incomingRequests;
        set => SetProperty(ref _incomingRequests, value);
    }

    public bool HasFriends          => Friends.Count > 0;
    public bool HasIncomingRequests => IncomingRequests.Count > 0;

    public Command LoadCommand              { get; }
    public Command SendRequestCommand       { get; }
    public Command<Guid> AcceptCommand      { get; }
    public Command<Guid> DeclineCommand     { get; }
    public Command<Guid> RemoveFriendCommand { get; }

    public FriendsViewModel(IFriendsApiClient friendsApi)
    {
        _friendsApi = friendsApi;

        LoadCommand        = new Command(async () => await LoadAsync());
        SendRequestCommand = new Command(async () => await SendRequestAsync());
        AcceptCommand      = new Command<Guid>(async id => await AcceptAsync(id));
        DeclineCommand     = new Command<Guid>(async id => await DeclineAsync(id));
        RemoveFriendCommand = new Command<Guid>(async id => await RemoveFriendAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Friends          = (await _friendsApi.GetFriendsAsync()) ?? [];
            IncomingRequests = (await _friendsApi.GetIncomingRequestsAsync()) ?? [];
            OnPropertyChanged(nameof(HasFriends));
            OnPropertyChanged(nameof(HasIncomingRequests));
        });
    }

    private async Task SendRequestAsync()
    {
        var input = await Shell.Current.DisplayPromptAsync(
            "Добавить друга", "Введите никнейм пользователя");
        if (string.IsNullOrWhiteSpace(input)) return;

        await RunSafeAsync(async () =>
        {
            await _friendsApi.SendRequestAsync(input.Trim());
            await Shell.Current.DisplayAlert("Отправлено", $"Запрос отправлен игроку «{input}»", "OK");
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
            await LoadAsync(); 
        });
    }

    private async Task RemoveFriendAsync(Guid friendUserId)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Удалить друга?",
            "Вы уверены? Дружба будет удалена с обеих сторон.",
            "Удалить", "Отмена");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            await _friendsApi.RemoveFriendAsync(friendUserId);
            await LoadAsync();
        });
    }
}