using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Support;

namespace MobileApp.Views.Support;

public partial class TicketChatPage : ContentPage
{
    private readonly TicketChatViewModel _vm;

    public TicketChatViewModel ViewModel => _vm;

    public TicketChatPage(TicketChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}