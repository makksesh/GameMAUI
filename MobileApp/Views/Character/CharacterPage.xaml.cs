using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Character;

namespace MobileApp.Views.Character;

public partial class CharacterPage : ContentPage
{
    private readonly CharacterViewModel _viewModel;

    public CharacterPage(CharacterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}