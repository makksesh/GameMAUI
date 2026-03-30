
using MobileApp.Models.Support;
using MobileApp.ViewModels.Support;

namespace MobileApp.Views.Support;

public partial class SupportPage : ContentPage
{
    private readonly SupportViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public SupportPage(SupportViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private async void OnTicketTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Border border || border.BindingContext is not SupportTicketDto ticket)
            return;

        var page = _serviceProvider.GetRequiredService<TicketChatPage>();

        page.ViewModel.TicketId = ticket.Id;
        page.ViewModel.TicketSubject = ticket.Subject;
        page.ViewModel.TicketStatus = ticket.Status;

        await Navigation.PushAsync(page);
    }
}