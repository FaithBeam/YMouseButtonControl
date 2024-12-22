using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Logging;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuInstall;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuUninstall;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.UpdateStartsMinimized;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartsMinimized;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog;

public interface IGlobalSettingsDialogViewModel;

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private StartsMinimized.StartsMinimizedResponse _startMinimizedSetting;
    private bool _loggingEnabled;
    private bool _startMenuChecked;
    private SettingIntVm _themeSetting;
    private ObservableCollection<ThemeVm> _themeCollection;
    private ThemeVm _selectedTheme;
    private readonly ObservableAsPropertyHelper<bool>? _applyIsExec;

    public GlobalSettingsDialogViewModel(
        IStartsMinimized startsMinimized,
        IUpdateStartsMinimized updateStartsMinimized,
        IStartMenuInstallerStatus startupMenuInstallerStatus,
        IStartMenuInstall startMenuInstall,
        IStartMenuUninstall startMenuUninstall,
        IEnableLoggingService enableLoggingService,
        ISettingsService settingsService,
        IThemeService themeService
    )
        : base(themeService)
    {
        StartMenuEnabled = !OperatingSystem.IsMacOS();
        _startMenuChecked = StartMenuEnabled && startupMenuInstallerStatus.InstallStatus();
        _startMinimizedSetting = startsMinimized.GetStartsMinimized();
        _loggingEnabled = enableLoggingService.GetLoggingState();
        _themeSetting =
            settingsService.GetSetting("Theme") as SettingIntVm
            ?? throw new Exception("Error retrieving Theme setting");
        _themeCollection = [.. themeService.Themes];
        _selectedTheme = _themeCollection.First(x => x.Id == _themeSetting.IntValue);

        // Update the theme setting selected theme value
        this.WhenAnyValue(x => x.SelectedTheme).Subscribe(x => ThemeSetting.IntValue = x.Id);

        var startMinimizedChanged = this.WhenAnyValue(
            x => x.StartMinimized.Value,
            selector: val => startsMinimized.GetStartsMinimized().Value != val
        );
        var loggingChanged = this.WhenAnyValue(
            x => x.LoggingEnabled,
            selector: val => val != enableLoggingService.GetLoggingState()
        );
        var startMenuChanged = this.WhenAnyValue(
            x => x.StartMenuChecked,
            selector: val => StartMenuEnabled && val != startupMenuInstallerStatus.InstallStatus()
        );
        var themeChanged = this.WhenAnyValue(
            x => x.ThemeSetting.IntValue,
            selector: val =>
                settingsService.GetSetting("Theme") is not SettingIntVm curVal
                || curVal.IntValue != val
        );
        var applyIsExecObs = this.WhenAnyValue(x => x.AppIsExec);
        var canSave = startMinimizedChanged
            .Merge(loggingChanged)
            .Merge(startMenuChanged)
            .Merge(applyIsExecObs)
            .Merge(themeChanged);
        ApplyCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                if (LoggingEnabled != enableLoggingService.GetLoggingState())
                {
                    if (LoggingEnabled)
                    {
                        enableLoggingService.EnableLogging();
                    }
                    else
                    {
                        enableLoggingService.DisableLogging();
                    }
                }

                if (
                    StartMenuEnabled
                    && StartMenuChecked != startupMenuInstallerStatus.InstallStatus()
                )
                {
                    if (StartMenuChecked)
                    {
                        startMenuInstall.Install();
                    }
                    else
                    {
                        startMenuUninstall.Uninstall();
                    }
                }

                await updateStartsMinimized.ExecuteAsync(
                    new UpdateStartsMinimized.Command(_startMinimizedSetting.Value)
                );
                settingsService.UpdateSetting(ThemeSetting);
            },
            canSave
        );
        _applyIsExec = ApplyCommand.IsExecuting.ToProperty(this, x => x.AppIsExec);
    }

    private bool AppIsExec => _applyIsExec?.Value ?? false;

    public bool StartMenuChecked
    {
        get => _startMenuChecked;
        set => this.RaiseAndSetIfChanged(ref _startMenuChecked, value);
    }

    public bool StartMenuEnabled { get; init; }

    public StartsMinimized.StartsMinimizedResponse StartMinimized
    {
        get => _startMinimizedSetting;
        set => this.RaiseAndSetIfChanged(ref _startMinimizedSetting, value);
    }

    public bool LoggingEnabled
    {
        get => _loggingEnabled;
        set => this.RaiseAndSetIfChanged(ref _loggingEnabled, value);
    }

    public ThemeVm SelectedTheme
    {
        get => _selectedTheme;
        set => this.RaiseAndSetIfChanged(ref _selectedTheme, value);
    }

    public SettingIntVm ThemeSetting
    {
        get => _themeSetting;
        set => this.RaiseAndSetIfChanged(ref _themeSetting, value);
    }

    public ObservableCollection<ThemeVm> ThemeCollection
    {
        get => _themeCollection;
        set => this.RaiseAndSetIfChanged(ref _themeCollection, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; init; }
}
