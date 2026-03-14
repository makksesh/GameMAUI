namespace MobileApp.ViewModels.Support;

public class ModeratorTicketItemViewModel : BaseViewModel
{
    public Guid Id { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string AuthorUsername { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Resolution { get; init; }

    private string _resolutionInput = string.Empty;
    public string ResolutionInput
    {
        get => _resolutionInput;
        set => SetProperty(ref _resolutionInput, value);
    }
}
