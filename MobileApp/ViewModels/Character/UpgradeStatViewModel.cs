using MobileApp.Models;
using MobileApp.Services.Api.Character;

namespace MobileApp.ViewModels.Character;

public class UpgradeStatViewModel : BaseViewModel
{
    private readonly ICharacterApiClient _characterApi;
    
    private string _selectedStat = "health";
    public string SelectedStat
    {
        get => _selectedStat;
        set => SetProperty(ref _selectedStat, value);
    }

    private int _amount = 1;
    public int Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, Math.Clamp(value, 1, 100));
    }
    
    public double AmountDouble
    {
        get => _amount;
        set
        {
            var clamped = Math.Clamp((int)Math.Round(value), 1, 100);
            if (_amount == clamped) return;
            _amount = clamped;
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(AmountDouble));
        }
    }
    
    

    private CharacterDto? _updated;
    public CharacterDto? Updated
    {
        get => _updated;
        set => SetProperty(ref _updated, value);
    }

    public List<string> Stats { get; } = ["health", "mana", "armor", "damage"];

    public Command UpgradeCommand { get; }
    public Command<string> SelectStatCommand { get; }
    public Command IncreaseCommand    { get; }
    public Command DecreaseCommand    { get; }

    public UpgradeStatViewModel(ICharacterApiClient characterApi)
    {
        _characterApi = characterApi;

        UpgradeCommand     = new Command(async () => await UpgradeAsync(), () => !IsBusy);
        SelectStatCommand  = new Command<string>(stat => SelectedStat = stat);
        IncreaseCommand    = new Command(() => Amount++);
        DecreaseCommand = new Command(() => { if (Amount > 1) Amount--; });
    }

    private async Task UpgradeAsync()
    {
        await RunSafeAsync(async () =>
        {
            Updated = await _characterApi.UpgradeStatAsync(new UpgradeStatRequest
            {
                Stat   = SelectedStat,
                Amount = Amount
            });
            await Shell.Current.DisplayAlert(
                "Успех ✨",
                $"{SelectedStat} улучшен на {Amount} очков!",
                "OK");
            await Shell.Current.GoToAsync("..");
        });
    }
}