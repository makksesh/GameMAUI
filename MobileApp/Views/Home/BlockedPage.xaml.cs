using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Home;

namespace MobileApp.Views.Home;

public partial class BlockedPage : ContentPage
{
    public BlockedPage(BlockedViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}