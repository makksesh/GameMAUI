using MobileApp.Models.Support;
using MobileApp.Services.Api.Support;

namespace MobileApp.ViewModels.Support;

public class TicketChatViewModel : BaseViewModel
{
    private readonly ISupportApiClient _supportApi;

    public Guid TicketId { get; set; }
    public string TicketSubject { get; set; } = string.Empty;
    public string TicketStatus { get; set; } = string.Empty;

    private List<SupportMessageDto> _messages = [];
    public List<SupportMessageDto> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }

    private string _newText = string.Empty;
    public string NewText
    {
        get => _newText;
        set
        {
            if (SetProperty(ref _newText, value))
                SendCommand.ChangeCanExecute();
        }
    }

    public Command SendCommand { get; }
    public Command LoadCommand { get; }

    public TicketChatViewModel(ISupportApiClient supportApi)
    {
        _supportApi = supportApi;

        LoadCommand = new Command(async () => await LoadAsync());
        SendCommand = new Command(
            async () => await SendAsync(),
            () => !IsBusy && !string.IsNullOrWhiteSpace(NewText));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            Messages = await _supportApi.GetMessagesAsync(TicketId) ?? [];
        });

        SendCommand.ChangeCanExecute();
    }

    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(NewText))
            return;

        await RunSafeAsync(async () =>
        {
            var msg = await _supportApi.SendMessageAsync(
                TicketId,
                new SendMessageRequest { Text = NewText });

            if (msg is null) return;

            Messages = [..Messages, msg];
            NewText = string.Empty;
        });

        SendCommand.ChangeCanExecute();
    }
}