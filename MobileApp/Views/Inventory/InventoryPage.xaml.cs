using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Inventory;

namespace MobileApp.Views.Inventory;

public partial class InventoryPage : ContentPage
{
    private readonly InventoryViewModel _viewModel;

    public InventoryPage(InventoryViewModel viewModel)
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