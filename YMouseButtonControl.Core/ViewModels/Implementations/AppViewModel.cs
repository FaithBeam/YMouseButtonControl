using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.Services;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.ViewModels.Interfaces;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.Views;

namespace YMouseButtonControl.Core.ViewModels.Implementations;

public class AppViewModel : ViewModelBase, IAppViewModel
{
    private bool _runAtStartupIsChecked;
    private bool _runAtStartupIsEnabled;
    private const string RunAtStartupChecked = "✅ ";
    private const string RunAtStartupNotChecked = "";
    private const string RunAtStartupHeaderFmt = "{0}Run at startup";
    private string _runAtStartupHeader = "";

    public AppViewModel(
        IStartupInstallerService startupInstallerService,
        IMainWindow mainWindow,
        IMainWindowViewModel mainWindowViewModel,
        IBackgroundTasksRunner backgroundTasksRunner
    )
    {
        RunAtStartupIsEnabled = startupInstallerService.ButtonEnabled();
        RunAtStartupIsChecked = startupInstallerService.InstallStatus();
        RunAtStartupHeader = RunAtStartupIsChecked
            ? string.Format(RunAtStartupHeaderFmt, RunAtStartupChecked)
            : string.Format(RunAtStartupHeaderFmt, RunAtStartupNotChecked);
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
                lifetime.MainWindow!.Opened -= HideWindow.Hide;
                lifetime.MainWindow?.Show();
            }
        });
        var runAtStartupCanExecute = this.WhenAnyValue(x => x.RunAtStartupIsEnabled);
        RunAtStartupCommand = ReactiveCommand.Create(
            () =>
            {
                if (startupInstallerService.InstallStatus())
                {
                    // uninstall
                    startupInstallerService.Uninstall();
                    RunAtStartupIsChecked = false;
                    RunAtStartupHeader = string.Format(
                        RunAtStartupHeaderFmt,
                        RunAtStartupNotChecked
                    );
                }
                else
                {
                    // install
                    startupInstallerService.Install();
                    RunAtStartupIsChecked = true;
                    RunAtStartupHeader = string.Format(RunAtStartupHeaderFmt, RunAtStartupChecked);
                }
            },
            runAtStartupCanExecute
        );
    }

    public string ToolTipText => $"YMouseButtonControl v{GetType().Assembly.GetName().Version}";

    public bool RunAtStartupIsEnabled
    {
        get => _runAtStartupIsEnabled;
        set => this.RaiseAndSetIfChanged(ref _runAtStartupIsEnabled, value);
    }

    public bool RunAtStartupIsChecked
    {
        get => _runAtStartupIsChecked;
        set => this.RaiseAndSetIfChanged(ref _runAtStartupIsChecked, value);
    }

    public string RunAtStartupHeader
    {
        get => _runAtStartupHeader;
        set => this.RaiseAndSetIfChanged(ref _runAtStartupHeader, value);
    }

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Unit, Unit> SetupCommand { get; }
    public ReactiveCommand<Unit, Unit> RunAtStartupCommand { get; }
}
