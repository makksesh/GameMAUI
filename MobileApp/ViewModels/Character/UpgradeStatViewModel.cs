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
        set => SetProperty(ref _amount, value);
    }

    private CharacterDto? _updated;
    public CharacterDto? Updated
    {
        get => _updated;
        set => SetProperty(ref _updated, value);
    }

    public List<string> Stats { get; } = ["health", "mana", "armor", "damage"];

    public Command UpgradeCommand { get; }

    public UpgradeStatViewModel(ICharacterApiClient characterApi)
    {
        _characterApi = characterApi;
        UpgradeCommand = new Command(async () => await UpgradeAsync(), () => !IsBusy);
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
            await Shell.Current.DisplayAlert("Успех", $"{SelectedStat} улучшен!", "OK");
        });
    }
}