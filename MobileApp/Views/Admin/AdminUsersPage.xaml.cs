using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Admin;

namespace MobileApp.Views.Admin;

public partial class AdminUsersPage : ContentPage
{
    private readonly AdminViewModel _viewModel;

    public AdminUsersPage(AdminViewModel viewModel)
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