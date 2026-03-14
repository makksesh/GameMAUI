using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MobileApp.Services.Storage;

namespace MobileApp;

public partial class App : Application
{
    public App(AppShell shell)
    {
        InitializeComponent();
        MainPage = shell;
    }
}