using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using YMouseButtonControl.Core.Services.StartupInstaller;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Commands.StartupInstaller.Install;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Commands.StartupInstaller.Uninstall;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.CanBeInstalled;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.IsInstalled;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.Views;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel;

public interface IAppViewModel;

public class AppViewModel : ViewModelBase, IAppViewModel
{
    private bool _runAtStartupIsChecked;
    private bool _runAtStartupIsEnabled;
    private const string RunAtStartupChecked = "✅ ";
    private const string RunAtStartupNotChecked = "";
    private const string RunAtStartupHeaderFmt = "{0}Run at startup";
    private string _runAtStartupHeader = "";

    public AppViewModel(
        ICanBeInstalledHandler canBeInstalledHandler,
        IIsInstalledHandler isInstalledHandler,
        IInstallHandler installHandler,
        IUninstallHandler uninstallHandler,
        IMainWindow mainWindow,
        IMainWindowViewModel mainWindowViewModel
    )
    {
        RunAtStartupIsEnabled = canBeInstalledHandler.Execute();
        RunAtStartupIsChecked = isInstalledHandler.Execute();
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
                if (lifetime.MainWindow is null)
                {
                    var mw = (Window)mainWindow;
                    mw.DataContext = mainWindowViewModel;
                    lifetime.MainWindow = mw;
                }
                lifetime.MainWindow?.Show();
            }
        });
        var runAtStartupCanExecute = this.WhenAnyValue(x => x.RunAtStartupIsEnabled);
        RunAtStartupCommand = ReactiveCommand.Create(
            () =>
            {
                if (isInstalledHandler.Execute())
                {
                    // uninstall
                    uninstallHandler.Execute();
                    RunAtStartupIsChecked = false;
                    RunAtStartupHeader = string.Format(
                        RunAtStartupHeaderFmt,
                        RunAtStartupNotChecked
                    );
                }
                else
                {
                    // install
                    installHandler.Execute();
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
