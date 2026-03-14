using System;
using MobileApp.Models.Inventory;
using MobileApp.Services.Api.Inventory;

namespace MobileApp.ViewModels.Inventory;

public class InventoryViewModel : BaseViewModel
{
    private readonly IInventoryApiClient _inventoryApi;

    private List<InventoryItemDto> _items = [];
    public List<InventoryItemDto> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public Command LoadCommand { get; }
    public Command<Guid> EquipCommand { get; }
    public Command<Guid> UnequipCommand { get; }

    public InventoryViewModel(IInventoryApiClient inventoryApi)
    {
        _inventoryApi = inventoryApi;

        LoadCommand    = new Command(async () => await LoadAsync());
        EquipCommand   = new Command<Guid>(async id => await EquipAsync(id));
        UnequipCommand = new Command<Guid>(async id => await UnequipAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
            Items = (await _inventoryApi.GetMyAsync()) ?? []);
    }

    private async Task EquipAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.EquipAsync(id);
            if (updated is null) return;
            Items = Items.Select(i => i.Id == id ? updated : i).ToList();
        });
    }

    private async Task UnequipAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.UnequipAsync(id);
            if (updated is null) return;
            Items = Items.Select(i => i.Id == id ? updated : i).ToList();
        });
    }
}