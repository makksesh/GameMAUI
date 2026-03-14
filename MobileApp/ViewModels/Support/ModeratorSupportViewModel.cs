using MobileApp.Models.Support;
using MobileApp.Services.Api.Support;

namespace MobileApp.ViewModels.Support;

public class ModeratorSupportViewModel : BaseViewModel
{
    private readonly ISupportApiClient _supportApi;

    private List<ModeratorTicketItemViewModel> _tickets = new();
    public List<ModeratorTicketItemViewModel> Tickets
    {
        get => _tickets;
        set => SetProperty(ref _tickets, value);
    }

    private string _selectedStatus = "New";
    public string SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (SetProperty(ref _selectedStatus, value))
            {
                LoadCommand.Execute(null);
            }
        }
    }

    public List<string> StatusOptions => new() { "New", "InProgress", "Resolved" };

    public Command LoadCommand { get; }
    public Command<Guid> AssignCommand { get; }
    public Command<Guid> ResolveCommand { get; }

    public ModeratorSupportViewModel(ISupportApiClient supportApi)
    {
        _supportApi = supportApi;
        LoadCommand = new Command(async () => await LoadAsync());
        AssignCommand = new Command<Guid>(async id => await AssignAsync(id));
        ResolveCommand = new Command<Guid>(async id => await ResolveAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            var data = await _supportApi.GetTicketsByStatusAsync(SelectedStatus) ?? new();
            Tickets = data.Select(t => MapToItem(t)).ToList();
        });
    }


    public async Task AssignAsync(Guid ticketId)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _supportApi.AssignToMeAsync(ticketId);
            if (updated is null) return;

            Tickets = Tickets
                .Select(t => t.Id == ticketId ? MapToItem(updated, t.ResolutionInput) : t)
                .ToList();
        });
    }

    public async Task ResolveAsync(Guid ticketId)
    {
        var ticket = Tickets.FirstOrDefault(t => t.Id == ticketId);
        if (ticket is null)
        {
            ErrorMessage = "Тикет не найден";
            return;
        }

        if (string.IsNullOrWhiteSpace(ticket.ResolutionInput))
        {
            ErrorMessage = "Введите текст резолюции";
            return;
        }

        await RunSafeAsync(async () =>
        {
            var updated = await _supportApi.ResolveTicketAsync(
                ticketId,
                new ResolveTicketRequest
                {
                    Resolution = ticket.ResolutionInput
                });

            if (updated is null) return;

            Tickets = Tickets.Where(t => t.Id != ticketId).ToList();
        });
    }

    private static ModeratorTicketItemViewModel MapToItem(
        SupportTicketDto ticket,
        string resolutionInput = "")
    {
        return new ModeratorTicketItemViewModel
        {
            Id = ticket.Id,
            Subject = ticket.Subject,
            AuthorUsername = ticket.AuthorUsername,
            Message = ticket.Message,
            Status = ticket.Status,
            Resolution = ticket.Resolution,
            ResolutionInput = resolutionInput
        };
    }
}
