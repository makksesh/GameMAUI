using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels.Support;

namespace MobileApp.Views.Support;

public partial class ModeratorSupportPage : ContentPage
{
    private readonly ModeratorSupportViewModel _vm;
    private readonly IServiceProvider _serviceProvider;

    public ModeratorSupportPage(
        ModeratorSupportViewModel vm,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private async void OnOpenChatClicked(object sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not ModeratorTicketItemViewModel ticket)
            return;

        var page = _serviceProvider.GetRequiredService<TicketChatPage>();

        page.ViewModel.TicketId = ticket.Id;
        page.ViewModel.TicketSubject = ticket.Subject;
        page.ViewModel.TicketStatus = ticket.Status;

        await Navigation.PushAsync(page);
    }
}