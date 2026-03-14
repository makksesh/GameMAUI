using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Auth;

namespace MobileApp.Views.Auth;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage(ChangePasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}