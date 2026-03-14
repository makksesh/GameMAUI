using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Friends;

namespace MobileApp.Views.Friends;

public partial class FriendsPage : ContentPage
{
    private readonly FriendsViewModel _viewModel;

    public FriendsPage(FriendsViewModel viewModel)
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