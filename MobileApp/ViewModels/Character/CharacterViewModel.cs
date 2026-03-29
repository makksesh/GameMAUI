using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.Models.Inventory;
using MobileApp.Services.Api.Character;
using MobileApp.Services.Api.Inventory;

namespace MobileApp.ViewModels.Character;

public class CharacterViewModel : BaseViewModel
{
    private readonly ICharacterApiClient _characterApi;
    private readonly IInventoryApiClient _inventoryApi;

    // ══════════════════════════════════
    // Персонаж
    // ══════════════════════════════════
    private CharacterDto? _character;
    public CharacterDto? Character
    {
        get => _character;
        set => SetProperty(ref _character, value);
    }
    public bool HasCharacter => Character is not null;

    // ══════════════════════════════════
    // Все предметы инвентаря
    // ══════════════════════════════════
    private ObservableCollection<InventoryItemDto> _allItems = [];
    public ObservableCollection<InventoryItemDto> AllItems
    {
        get => _allItems;
        set
        {
            SetProperty(ref _allItems, value);
            NotifyEquipmentSlots();
        }
    }

    // ══════════════════════════════════
    // Слоты оружия (макс. 3)
    // ══════════════════════════════════
    public InventoryItemDto WeaponSlot1 => GetSlot("Weapon", 0);
    public InventoryItemDto WeaponSlot2 => GetSlot("Weapon", 1);
    public InventoryItemDto WeaponSlot3 => GetSlot("Weapon", 2);

    // ══════════════════════════════════
    // Слоты колец (макс. 2)
    // ══════════════════════════════════
    public InventoryItemDto RingSlot1 => GetSlot("Ring", 0);
    public InventoryItemDto RingSlot2 => GetSlot("Ring", 1);

    // ══════════════════════════════════
    // Одиночные слоты брони
    // ══════════════════════════════════
    public InventoryItemDto HelmetSlot => GetSlotOrEmpty("Helmet");
    public InventoryItemDto ArmorSlot  => GetSlotOrEmpty("Armor");
    public InventoryItemDto BootsSlot  => GetSlotOrEmpty("Boots");

    // ══════════════════════════════════
    // Команды
    // ══════════════════════════════════
    public Command LoadCommand        { get; }
    public Command CreateCommand      { get; }
    public Command GoToUpgradeCommand { get; }
    public Command<Guid> EquipCommand   { get; }
    public Command<Guid> UnequipCommand { get; }

    public CharacterViewModel(
        ICharacterApiClient characterApi,
        IInventoryApiClient inventoryApi)
    {
        _characterApi = characterApi;
        _inventoryApi = inventoryApi;

        LoadCommand        = new Command(async () => await LoadAsync());
        CreateCommand      = new Command(async () => await CreateAsync(), () => !IsBusy);
        GoToUpgradeCommand = new Command(async () => await Shell.Current.GoToAsync("UpgradeStatPage"));
        EquipCommand       = new Command<Guid>(async id => await EquipAsync(id));
        UnequipCommand     = new Command<Guid>(async id => await UnequipAsync(id));
    }

    // ══════════════════════════════════
    // Загрузка данных
    // ══════════════════════════════════
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Character = await _characterApi.GetMyAsync();
            var items = (await _inventoryApi.GetMyAsync()) ?? [];
            // Временный дебаг
            Console.WriteLine($"[DEBUG] Загружено предметов: {items.Count}");
            foreach (var item in items)
                Console.WriteLine($"[DEBUG] {item.ItemName} | {item.ItemType} | Equipped: {item.IsEquipped}");
            AllItems = new ObservableCollection<InventoryItemDto>(items);
            OnPropertyChanged(nameof(HasCharacter));
            
        });
    }

    private async Task CreateAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync(
            "Создать персонажа", "Введите имя персонажа");

        if (string.IsNullOrWhiteSpace(name)) return;

        await RunSafeAsync(async () =>
        {
            Character = await _characterApi.CreateAsync(
                new CreateCharacterRequest { Name = name });
            OnPropertyChanged(nameof(HasCharacter));
        });
    }

    private async Task EquipAsync(Guid id)
    {
        if (id == Guid.Empty) return;
        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.EquipAsync(id);
            if (updated is null) return;
            var index = AllItems.IndexOf(AllItems.First(i => i.Id == id));
            AllItems[index] = updated;
            NotifyEquipmentSlots();
        });
    }

    private async Task UnequipAsync(Guid id)
    {
        if (id == Guid.Empty) return;
        await RunSafeAsync(async () =>
        {
            var updated = await _inventoryApi.UnequipAsync(id);
            if (updated is null) return;
            var index = AllItems.IndexOf(AllItems.First(i => i.Id == id));
            AllItems[index] = updated;
            NotifyEquipmentSlots();
        });
    }

    // ══════════════════════════════════
    // Хелперы слотов
    // ══════════════════════════════════

    /// <summary>Возвращает n-й надетый предмет нужного типа или пустую заглушку.</summary>
    private InventoryItemDto GetSlot(string type, int index)
    {
        var equipped = _allItems
            .Where(i => i.ItemType == type && i.IsEquipped)
            .ToList();

        return index < equipped.Count
            ? equipped[index]
            : EmptySlot(type);
    }

    /// <summary>Первый надетый предмет нужного типа или пустая заглушка.</summary>
    private InventoryItemDto GetSlotOrEmpty(string type) =>
        _allItems.FirstOrDefault(i => i.ItemType == type && i.IsEquipped)
        ?? EmptySlot(type);

    private static InventoryItemDto EmptySlot(string type) =>
        new() { Id = Guid.Empty, ItemName = string.Empty, ItemType = type };

    private void NotifyEquipmentSlots()
    {
        OnPropertyChanged(nameof(WeaponSlot1));
        OnPropertyChanged(nameof(WeaponSlot2));
        OnPropertyChanged(nameof(WeaponSlot3));
        OnPropertyChanged(nameof(RingSlot1));
        OnPropertyChanged(nameof(RingSlot2));
        OnPropertyChanged(nameof(HelmetSlot));
        OnPropertyChanged(nameof(ArmorSlot));
        OnPropertyChanged(nameof(BootsSlot));
    }
}
