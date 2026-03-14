using MobileApp.Models;
using MobileApp.Services.Api.Character;

namespace MobileApp.ViewModels.Character;

public class CharacterViewModel : BaseViewModel
{
    private readonly ICharacterApiClient _characterApi;

    private CharacterDto? _character;
    public CharacterDto? Character
    {
        get => _character;
        set => SetProperty(ref _character, value);
    }

    public bool HasCharacter => Character is not null;

    public Command LoadCommand { get; }
    public Command CreateCommand { get; }
    public Command GoToUpgradeCommand { get; }

    public CharacterViewModel(ICharacterApiClient characterApi)
    {
        _characterApi = characterApi;

        LoadCommand = new Command(async () => await LoadAsync());
        CreateCommand = new Command(async () => await CreateAsync(), () => !IsBusy);
        GoToUpgradeCommand = new Command(async () =>
            await Shell.Current.GoToAsync("UpgradeStatPage"));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Character = await _characterApi.GetMyAsync();
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
            Character = await _characterApi.CreateAsync(new CreateCharacterRequest { Name = name });
            OnPropertyChanged(nameof(HasCharacter));
        });
    }
}