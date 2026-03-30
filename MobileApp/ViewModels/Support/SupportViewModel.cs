using System;
using MobileApp.Models.Support;
using MobileApp.Services.Api.Support;

namespace MobileApp.ViewModels.Support;

public class SupportViewModel : BaseViewModel
{
    private readonly ISupportApiClient _supportApi;

    public List<SupportTicketDto> Tickets { get => _tickets; set => SetProperty(ref _tickets, value); }
    private List<SupportTicketDto> _tickets = [];

    public string NewSubject
    {
        get => _subject;
        set
        {
            SetProperty(ref _subject, value);
            NotifyCommandsCanExecuteChanged();
        }
    }
    public string NewMessage
    {
        get => _message;
        set
        {
            SetProperty(ref _message, value);
            NotifyCommandsCanExecuteChanged();
        }
    }
    private string _subject = string.Empty;
    private string _message = string.Empty;

    public Command LoadCommand { get; }
    public Command CreateCommand { get; }
    public Command<Guid> CloseCommand { get; }
    public Command<Guid> ReopenCommand { get; }

    public SupportViewModel(ISupportApiClient supportApi)
    {
        _supportApi = supportApi;

        LoadCommand   = new Command(async () => await LoadAsync());
        CreateCommand = new Command(
            async () => await CreateAsync(),
            () => !IsBusy
                  && !string.IsNullOrWhiteSpace(NewSubject)
                  && !string.IsNullOrWhiteSpace(NewMessage));
        CloseCommand  = new Command<Guid>(async id => await CloseAsync(id));
        ReopenCommand = new Command<Guid>(async id => await ReopenAsync(id));
        
        Track(CreateCommand);
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
            Tickets = (await _supportApi.GetMyTicketsAsync()) ?? []);
    }

    private async Task CreateAsync()
    {
        await RunSafeAsync(async () =>
        {
            var ticket = await _supportApi.CreateTicketAsync(new CreateTicketRequest
            {
                Subject = NewSubject,
                Message = NewMessage
            });
            if (ticket is null) return;

            // Сразу отправляем описание как первое сообщение в чат
            await _supportApi.SendMessageAsync(ticket.Id, new SendMessageRequest
            {
                Text = NewMessage
            });

            Tickets = [ticket, ..Tickets];
            NewSubject = string.Empty;
            NewMessage = string.Empty;
        });
    }

    private async Task CloseAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            await _supportApi.CloseTicketAsync(id);
            await LoadAsync();
        });
    }

    private async Task ReopenAsync(Guid id)
    {
        await RunSafeAsync(async () =>
        {
            await _supportApi.ReopenTicketAsync(id);
            await LoadAsync();
        });
    }
}