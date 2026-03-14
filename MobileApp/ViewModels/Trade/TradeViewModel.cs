using System;
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
    private Guid _currentCharacterId;
    public Guid CurrentCharacterId
    {
        get => _currentCharacterId;
        private set => SetProperty(ref _currentCharacterId, value);
    }

    // Инвентарь для Picker 
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

    private List<TradeLotDto> _lots = [];
    public List<TradeLotDto> Lots
    {
        get => _lots;
        set => SetProperty(ref _lots, value);
    }

    public int NewQuantity   { get => _quantity; set => SetProperty(ref _quantity, value); }

    private int     _quantity = 1;
    private decimal _price;

    public Command LoadCommand { get; }
    public Command CreateLotCommand { get; }
    public Command<Guid> BuyCommand { get; }
    public Command<Guid> CancelCommand { get; }
    public Command IncreaseQuantityCommand { get; }
    public Command DecreaseQuantityCommand { get; }
    public Command GoToCreateLotCommand { get; }
    public Command GoBackCommand { get; }

    public TradeViewModel(ITradeApiClient tradeApi, IInventoryApiClient inventoryApi, ISessionService session, ICharacterApiClient characterApi) 
    {
        _tradeApi = tradeApi;
        _inventoryApi = inventoryApi; 
        _characterApi = characterApi;
        _session = session;

        LoadCommand      = new Command(async () => await LoadAsync());
        CreateLotCommand = new Command(async () => await CreateLotAsync(), () => !IsBusy);
        BuyCommand       = new Command<Guid>(async id => await BuyAsync(id));
        CancelCommand    = new Command<Guid>(async id => await CancelAsync(id));
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        GoToCreateLotCommand = new Command(async () =>
            await Shell.Current.GoToAsync("CreateTradeLotPage"));
        IncreaseQuantityCommand = new Command(() => NewQuantity++);
        DecreaseQuantityCommand = new Command(() =>
        {
            if (NewQuantity > 1) NewQuantity--;
        });
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Lots = (await _tradeApi.GetActiveLotsAsync()) ?? [];
            InventoryItems = (await _inventoryApi.GetMyAsync()) ?? []; 
            var character = await _characterApi.GetMyAsync();
            CurrentCharacterId = character?.Id ?? Guid.Empty; 
        });
    }

    private async Task CreateLotAsync()
    {
        Console.WriteLine($"[DEBUG] SelectedInventoryItem: {SelectedInventoryItem?.ItemName ?? "null"}");
        Console.WriteLine($"[DEBUG] NewPriceText: {NewPriceText}");
        Console.WriteLine($"[DEBUG] NewQuantity: {NewQuantity}");

        if (SelectedInventoryItem is null)
        {
            ErrorMessage = "Выберите предмет из инвентаря";
            return;
        }

        if (!decimal.TryParse(NewPriceText, out var price) || price <= 0)
        {
            ErrorMessage = "Некорректная цена";
            return;
        }

        Console.WriteLine($"[DEBUG] Sending ItemId: {SelectedInventoryItem.ItemId}, Price: {price}");

        await RunSafeAsync(async () =>
        {
            var lot = await _tradeApi.CreateLotAsync(new CreateTradeLotRequest
            {
                ItemId   = SelectedInventoryItem.ItemId,
                Quantity = NewQuantity,
                Price    = price
            });

            Console.WriteLine($"[DEBUG] lot result: {lot?.Id.ToString() ?? "null"}");

            if (lot is null) return;

            Lots = [lot, ..Lots];
            await Shell.Current.GoToAsync("..");
        });
    }


    private async Task BuyAsync(Guid lotId)
    {
        await RunSafeAsync(async () =>
        {
            await _tradeApi.BuyAsync(new BuyItemRequest { TradeLotId = lotId });
            Lots = Lots.Where(l => l.Id != lotId).ToList();
        });
    }
    

    private async Task CancelAsync(Guid lotId)
    {
        await RunSafeAsync(async () =>
        {
            await _tradeApi.CancelLotAsync(lotId);
            Lots = Lots.Where(l => l.Id != lotId).ToList();
        });
    }
}