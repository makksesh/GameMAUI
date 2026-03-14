using System;
using MobileApp.Models.Admin;
using MobileApp.Services.Api.Admin;

namespace MobileApp.ViewModels.Admin;

public class AdminViewModel : BaseViewModel
{
    private readonly IAdminApiClient _adminApi;

    public List<UserDto> Users { get => _users; set => SetProperty(ref _users, value); }
    private List<UserDto> _users = [];

    public Command LoadCommand { get; }
    public Command<Guid> BlockCommand { get; }
    public Command<Guid> UnblockCommand { get; }

    public AdminViewModel(IAdminApiClient adminApi)
    {
        _adminApi = adminApi;

        LoadCommand    = new Command(async () => await LoadAsync());
        BlockCommand   = new Command<Guid>(async id => await BlockAsync(id));
        UnblockCommand = new Command<Guid>(async id => await UnblockAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
            Users = (await _adminApi.GetAllUsersAsync()) ?? []);
    }

    private async Task BlockAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _adminApi.BlockUserAsync(id);
            if (updated is null) return;
            Users = Users.Select(u => u.Id == id ? updated : u).ToList();
        });
    }

    private async Task UnblockAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _adminApi.UnblockUserAsync(id);
            if (updated is null) return;
            Users = Users.Select(u => u.Id == id ? updated : u).ToList();
        });
    }
}