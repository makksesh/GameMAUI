using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Trade;

namespace MobileApp.Views.Trade;

public partial class TradeLotsPage : ContentPage
{
    private readonly TradeViewModel _vm;

    public TradeLotsPage(TradeViewModel vm)
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