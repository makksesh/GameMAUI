using System;
using System.Collections.ObjectModel;
using MobileApp.Models.Inventory;
using MobileApp.Models.Trade;
using MobileApp.Services.Api.Character;
using MobileApp.Services.Api.Inventory;
using MobileApp.Services.Api.Trade;
using MobileApp.Services.Storage;

namespace MobileApp.ViewModels.Trade;

public class TradeViewModel : BaseViewModel
{
    private readonly ITradeApiClient _tradeApi;
    private readonly IInventoryApiClient _inventoryApi;
    private readonly ISessionService _session;
    private readonly ICharacterApiClient _characterApi;

    // ══════════════════════════════════
    // Персонаж
    // ══════════════════════════════════
    private Guid _currentCharacterId;
    public Guid CurrentCharacterId
    {
        get => _currentCharacterId;
        private set => SetProperty(ref _currentCharacterId, value);
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        private set => SetProperty(ref _balance, value);
    }

    // ══════════════════════════════════
    // Все лоты с сервера
    // ══════════════════════════════════
    private List<TradeLotDto> _allLots = [];

    // ══════════════════════════════════
    // Отфильтрованные лоты (показываем в UI)
    // ══════════════════════════════════
    private ObservableCollection<TradeLotDto> _lots = [];
    public ObservableCollection<TradeLotDto> Lots
    {
        get => _lots;
        set => SetProperty(ref _lots, value);
    }

    // ══════════════════════════════════
    // Поиск
    // ══════════════════════════════════
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            ApplyFilters();
        }
    }

    // ══════════════════════════════════
    // Фильтры
    // ══════════════════════════════════
    public List<string> RarityOptions { get; } =
        ["Все", "Common", "Uncommon", "Rare", "Epic", "Legendary"];

    public List<string> TypeOptions { get; } =
        ["Все", "Weapon", "Armor", "Helmet", "Boots", "Ring", "Potion", "Material", "QuestItem"];

    public List<string> SortOptions { get; } =
        ["По умолчанию", "Цена ↑", "Цена ↓"];

    private string _selectedRarity = "Все";
    public string SelectedRarity
    {
        get => _selectedRarity;
        set { SetProperty(ref _selectedRarity, value); ApplyFilters(); }
    }

    private string _selectedType = "Все";
    public string SelectedType
    {
        get => _selectedType;
        set { SetProperty(ref _selectedType, value); ApplyFilters(); }
    }

    private string _selectedSort = "По умолчанию";
    public string SelectedSort
    {
        get => _selectedSort;
        set { SetProperty(ref _selectedSort, value); ApplyFilters(); }
    }

    // ══════════════════════════════════
    // Инвентарь для создания лота
    // ══════════════════════════════════
    private List<InventoryItemDto> _inventoryItems = [];
    public List<InventoryItemDto> InventoryItems
    {
        get => _inventoryItems;
        set => SetProperty(ref _inventoryItems, value);
    }

    private InventoryItemDto? _selectedInventoryItem;
    public InventoryItemDto? SelectedInventoryItem
    {
        get => _selectedInventoryItem;
        set
        {
            SetProperty(ref _selectedInventoryItem, value);
            CreateLotCommand.ChangeCanExecute();
        }
    }

    private string _priceText = string.Empty;
    public string NewPriceText
    {
        get => _priceText;
        set => SetProperty(ref _priceText, value);
    }

    public int NewQuantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    private int _quantity = 1;

    // ══════════════════════════════════
    // Команды
    // ══════════════════════════════════
    public Command LoadCommand             { get; }
    public Command CreateLotCommand        { get; }
    public Command<Guid> BuyCommand        { get; }
    public Command<Guid> CancelCommand     { get; }
    public Command IncreaseQuantityCommand { get; }
    public Command DecreaseQuantityCommand { get; }
    public Command GoToCreateLotCommand    { get; }
    public Command GoBackCommand           { get; }
    public Command ShowFiltersCommand      { get; }
    public Command ResetFiltersCommand     { get; }

    public TradeViewModel(
        ITradeApiClient tradeApi,
        IInventoryApiClient inventoryApi,
        ISessionService session,
        ICharacterApiClient characterApi)
    {
        _tradeApi = tradeApi;
        _inventoryApi = inventoryApi;
        _characterApi = characterApi;
        _session = session;

        LoadCommand             = new Command(async () => await LoadAsync());
        CreateLotCommand        = new Command(async () => await CreateLotAsync(), () => !IsBusy);
        BuyCommand              = new Command<Guid>(async id => await BuyAsync(id));
        CancelCommand           = new Command<Guid>(async id => await CancelAsync(id));
        GoBackCommand           = new Command(async () => await Shell.Current.GoToAsync(".."));
        GoToCreateLotCommand    = new Command(async () => await Shell.Current.GoToAsync("CreateTradeLotPage"));
        IncreaseQuantityCommand = new Command(() => NewQuantity++);
        DecreaseQuantityCommand = new Command(() => { if (NewQuantity > 1) NewQuantity--; });
        ShowFiltersCommand      = new Command(async () => await ShowFiltersAsync());
        ResetFiltersCommand     = new Command(ResetFilters);
    }

    // ══════════════════════════════════
    // Загрузка
    // ══════════════════════════════════
    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            _allLots = (await _tradeApi.GetActiveLotsAsync()) ?? [];
            InventoryItems = (await _inventoryApi.GetMyAsync()) ?? [];
            var character = await _characterApi.GetMyAsync();
            CurrentCharacterId = character?.Id ?? Guid.Empty;
            Balance = character?.Balance ?? 0;
            ApplyFilters();
        });
    }

    // ══════════════════════════════════
    // Фильтрация
    // ══════════════════════════════════
    private void ApplyFilters()
    {
        var result = _allLots.AsEnumerable();

        // Поиск по названию
        if (!string.IsNullOrWhiteSpace(SearchText))
            result = result.Where(l =>
                l.ItemName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        // Фильтр редкости
        if (SelectedRarity != "Все")
            result = result.Where(l => l.ItemRarity == SelectedRarity);

        // Фильтр типа
        // result = result.Where(l => l.ItemType == SelectedType);

        // Сортировка
        result = SelectedSort switch
        {
            "Цена ↑" => result.OrderBy(l => l.Price),
            "Цена ↓" => result.OrderByDescending(l => l.Price),
            _        => result
        };

        Lots = new ObservableCollection<TradeLotDto>(result);
    }

    // ══════════════════════════════════
    // Модальное окно фильтров
    // ══════════════════════════════════
    private async Task ShowFiltersAsync()
    {
        // Редкость
        var rarity = await Shell.Current.DisplayActionSheet(
            "Редкость", "Отмена", null,
            RarityOptions.ToArray());
        if (!string.IsNullOrEmpty(rarity) && rarity != "Отмена")
            SelectedRarity = rarity;

        // Сортировка
        var sort = await Shell.Current.DisplayActionSheet(
            "Сортировка", "Отмена", null,
            SortOptions.ToArray());
        if (!string.IsNullOrEmpty(sort) && sort != "Отмена")
            SelectedSort = sort;
    }

    private void ResetFilters()
    {
        _selectedRarity = "Все";
        _selectedType   = "Все";
        _selectedSort   = "По умолчанию";
        OnPropertyChanged(nameof(SelectedRarity));
        OnPropertyChanged(nameof(SelectedType));
        OnPropertyChanged(nameof(SelectedSort));
        ApplyFilters();
    }

    // ══════════════════════════════════
    // Покупка с подтверждением и проверкой баланса
    // ══════════════════════════════════
    private async Task BuyAsync(Guid lotId)
    {
        var lot = _allLots.FirstOrDefault(l => l.Id == lotId);
        if (lot is null) return;

        // Проверка баланса на клиенте (дополнительная защита)
        if (Balance < lot.Price)
        {
            await Shell.Current.DisplayAlert(
                "Недостаточно монет",
                $"Нужно {lot.Price:F0} G, у вас {Balance:F0} G",
                "OK");
            return;
        }

        // Подтверждение покупки
        bool confirm = await Shell.Current.DisplayAlert(
            "Подтверждение",
            $"Купить «{lot.ItemName}» за {lot.Price:F0} G?",
            "Купить", "Отмена");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            await _tradeApi.BuyAsync(new BuyItemRequest { TradeLotId = lotId });
            Balance -= lot.Price;
            _allLots = _allLots.Where(l => l.Id != lotId).ToList();
            ApplyFilters();
        });
    }

    private async Task CancelAsync(Guid lotId)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Отменить лот?", "Предмет вернётся в инвентарь", "Да", "Нет");
        if (!confirm) return;

        await RunSafeAsync(async () =>
        {
            await _tradeApi.CancelLotAsync(lotId);
            _allLots = _allLots.Where(l => l.Id != lotId).ToList();
            ApplyFilters();
        });
    }

    private async Task CreateLotAsync()
    {
        if (SelectedInventoryItem is null) { ErrorMessage = "Выберите предмет"; return; }
        if (!decimal.TryParse(NewPriceText, out var price) || price <= 0)
        { ErrorMessage = "Некорректная цена"; return; }

        await RunSafeAsync(async () =>
        {
            var lot = await _tradeApi.CreateLotAsync(new CreateTradeLotRequest
            {
                ItemId   = SelectedInventoryItem.ItemId,
                Quantity = NewQuantity,
                Price    = price
            });
            if (lot is null) return;
            _allLots = [lot, .._allLots];
            ApplyFilters();
            await Shell.Current.GoToAsync("..");
        });
    }
}