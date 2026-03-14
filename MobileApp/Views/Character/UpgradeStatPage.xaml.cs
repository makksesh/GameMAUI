using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Character;

namespace MobileApp.Views.Character;

public partial class UpgradeStatPage : ContentPage
{
    public UpgradeStatPage(UpgradeStatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}