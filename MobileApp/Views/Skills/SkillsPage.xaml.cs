using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Skills;

namespace MobileApp.Views.Skills;

public partial class SkillsPage : ContentPage
{
    private readonly SkillsViewModel _viewModel;

    public SkillsPage(SkillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private void ShowAll_Clicked(object sender, EventArgs e)
    {
        AllSkillsView.IsVisible = true;
        MySkillsView.IsVisible  = false;
    }

    private void ShowMy_Clicked(object sender, EventArgs e)
    {
        AllSkillsView.IsVisible = false;
        MySkillsView.IsVisible  = true;
    }
}