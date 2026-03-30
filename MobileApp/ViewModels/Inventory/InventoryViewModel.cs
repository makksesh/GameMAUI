using System;
using MobileApp.Models;
using MobileApp.Models.Inventory;
using MobileApp.Services.Api.Character;
using MobileApp.Services.Api.Inventory;

namespace MobileApp.ViewModels.Inventory;

public class InventoryViewModel : BaseViewModel
{
    private readonly IInventoryApiClient _inventoryApi;
    private readonly ICharacterApiClient _characterApi;

    // ── Характеристики персонажа ──
    private CharacterDto? _character;
    public CharacterDto? Character
    {
        get => _character;
        set => SetProperty(ref _character, value);
    }

    // ── Слоты экипировки ──
    private InventoryItemDto? _helmetSlot;
    public InventoryItemDto? HelmetSlot
    {
        get => _helmetSlot;
        set => SetProperty(ref _helmetSlot, value);
    }

    private InventoryItemDto? _armorSlot;
    public InventoryItemDto? ArmorSlot
    {
        get => _armorSlot;
        set => SetProperty(ref _armorSlot, value);
    }

    private InventoryItemDto? _bootsSlot;
    public InventoryItemDto? BootsSlot
    {
        get => _bootsSlot;
        set => SetProperty(ref _bootsSlot, value);
    }

    private InventoryItemDto? _ringSlot1;
    public InventoryItemDto? RingSlot1
    {
        get => _ringSlot1;
        set => SetProperty(ref _ringSlot1, value);
    }

    private InventoryItemDto? _ringSlot2;
    public InventoryItemDto? RingSlot2
    {
        get => _ringSlot2;
        set => SetProperty(ref _ringSlot2, value);
    }

    private InventoryItemDto? _weaponSlot;
    public InventoryItemDto? WeaponSlot
    {
        get => _weaponSlot;
        set => SetProperty(ref _weaponSlot, value);
    }

    // ── Сетка инвентаря (32 ячейки — только НЕ экипированные предметы) ──
    private List<InventorySlot> _inventorySlots = [];
    public List<InventorySlot> InventorySlots
    {
        get => _inventorySlots;
        set => SetProperty(ref _inventorySlots, value);
    }

    // ── Команды ──
    public Command LoadCommand { get; }
    public Command<Guid> EquipCommand { get; }
    public Command<Guid> UnequipCommand { get; }
    public Command GoToUpgradeCommand { get; }

    private const int InventorySize = 32;

    public InventoryViewModel(IInventoryApiClient inventoryApi, ICharacterApiClient characterApi)
    {
        _inventoryApi  = inventoryApi;
        _characterApi  = characterApi;

        LoadCommand       = new Command(async () => await LoadAsync());
        EquipCommand      = new Command<Guid>(async id => await EquipAsync(id));
        UnequipCommand    = new Command<Guid>(async id => await UnequipAsync(id));
        GoToUpgradeCommand = new Command(async () => await Shell.Current.GoToAsync("UpgradeStatPage"));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            // Грузим персонажа для характеристик
            Character = await _characterApi.GetMyAsync();

            // Грузим все предметы инвентаря
            var allItems = await _inventoryApi.GetMyAsync() ?? [];
            RebuildSlots(allItems);
        });
    }

    /// <summary>
    /// Распределяет предметы по слотам экипировки и ячейкам инвентаря.
    /// Экипированные → в слоты, не экипированные → в 32-ячеечную сетку.
    /// </summary>
    private void RebuildSlots(List<InventoryItemDto> allItems)
    {
        // Сбрасываем слоты
        HelmetSlot = null;
        ArmorSlot  = null;
        BootsSlot  = null;
        RingSlot1  = null;
        RingSlot2  = null;
        WeaponSlot = null;

        var unequipped = new List<InventoryItemDto>();

        foreach (var item in allItems)
        {
            if (item.IsEquipped)
            {
                switch (item.ItemType)
                {
                    case "Helmet": HelmetSlot = item; break;
                    case "Armor":  ArmorSlot  = item; break;
                    case "Boots":  BootsSlot  = item; break;
                    case "Ring":
                        if (RingSlot1 is null) RingSlot1 = item;
                        else RingSlot2 = item;
                        break;
                    case "Weapon": WeaponSlot = item; break;
                    // Prочие типы экипированных — в инвентарь не попадают
                }
            }
            else
            {
                unequipped.Add(item);
            }
        }

        // Строим 32 ячейки: заполняем предметами, остальные — пустые
        var slots = new List<InventorySlot>(InventorySize);
        for (int i = 0; i < InventorySize; i++)
        {
            slots.Add(new InventorySlot
            {
                Item = i < unequipped.Count ? unequipped[i] : null
            });
        }
        InventorySlots = slots;
    }

    private async Task EquipAsync(Guid id)
    {
        // Пустая ячейка передаёт Guid.Empty — игнорируем
        if (id == Guid.Empty) return;

        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.EquipAsync(id);
            if (updated is null) return;
            var allItems = await _inventoryApi.GetMyAsync() ?? [];
            RebuildSlots(allItems);
        });
    }

    private async Task UnequipAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.UnequipAsync(id);
            if (updated is null) return;
            var allItems = await _inventoryApi.GetMyAsync() ?? [];
            RebuildSlots(allItems);
        });
    }
}