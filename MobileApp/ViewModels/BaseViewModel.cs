using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileApp.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
                NotifyCommandsCanExecuteChanged(); // ← при каждом изменении IsBusy
        }
    }
    
    // Список команд, которые зависят от IsBusy
    private readonly List<Command> _trackedCommands = [];

    // Регистрируем команду — вызывай в конструкторе дочернего VM
    protected void Track(params Command[] commands)
        => _trackedCommands.AddRange(commands);
    
    protected void NotifyCommandsCanExecuteChanged()
    {
        foreach (var cmd in _trackedCommands)
            cmd.ChangeCanExecute();
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        OnPropertyChanged(nameof(HasError)); 
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    protected async Task RunSafeAsync(Func<Task> action)
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            await action();
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Ошибка сети: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}