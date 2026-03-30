using MobileApp.ViewModels.Home;

namespace MobileApp.Views.Home;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;

    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        Console.WriteLine($"OnAppearing() in HomePage");
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}