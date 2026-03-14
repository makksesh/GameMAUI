using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Trade;

namespace MobileApp.Views.Trade;

public partial class TradeLotsPage : ContentPage
{
    private readonly TradeViewModel _viewModel;

    public TradeLotsPage(TradeViewModel viewModel)
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