using MobileApp.ViewModels.Character;

namespace MobileApp.Views.Character;

public partial class CharacterPage : ContentPage
{
    private readonly CharacterViewModel _vm;

    public CharacterPage(CharacterViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}