using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Trade;

namespace MobileApp.Views.Trade;

public partial class CreateTradeLotPage : ContentPage
{
    private readonly TradeViewModel _vm;

    public CreateTradeLotPage(TradeViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadCommand.Execute(null); 
    }
}