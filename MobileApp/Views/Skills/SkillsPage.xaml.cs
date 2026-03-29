using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Skills;

namespace MobileApp.Views.Skills;

public partial class SkillsPage : ContentPage
{
    private readonly SkillsViewModel viewModel;

    public SkillsPage(SkillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (viewModel.AllSkills.Count == 0 && viewModel.MySkills.Count == 0)
            await viewModel.LoadAsync();
    }
}