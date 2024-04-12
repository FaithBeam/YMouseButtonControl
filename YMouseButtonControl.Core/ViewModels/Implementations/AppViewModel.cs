using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.Implementations;

public class AppViewModel : ViewModelBase, IAppViewModel
{
    public AppViewModel()
    {
        ExitCommand = ReactiveCommand.Create(() =>
        {
            if (
                Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime lifetime
            )
            {
                lifetime.Shutdown();
            }
        });
        SetupCommand = ReactiveCommand.Create(() =>
        {
            if (
                Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime lifetime
            )
            {
                lifetime.MainWindow?.Show();
            }
        });
    }

    public string ToolTipText => $"YMouseButtonControl v{GetType().Assembly.GetName().Version}";

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Unit, Unit> SetupCommand { get; }
}
