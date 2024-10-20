using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Transactions;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Logging;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.StartMenuInstaller;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IGlobalSettingsDialogViewModel;

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private SettingBoolVm _startMinimizedSetting;
    private bool _loggingEnabled;
    private bool _startMenuChecked;
    private SettingIntVm _themeSetting;
    private ObservableCollection<ThemeVm> _themeCollection;
    private ThemeVm _selectedTheme;
    private readonly ObservableAsPropertyHelper<bool>? _applyIsExec;

    public GlobalSettingsDialogViewModel(
        IStartMenuInstallerService startMenuInstallerService,
        IEnableLoggingService enableLoggingService,
        ISettingsService settingsService,
        IThemeService themeService
    )
        : base(themeService)
    {
        StartMenuEnabled = !OperatingSystem.IsMacOS();
        _startMenuChecked = StartMenuEnabled && startMenuInstallerService.InstallStatus();
        _startMinimizedSetting =
            settingsService.GetSetting("StartMinimized") as SettingBoolVm
            ?? throw new Exception("Error retrieving StartMinimized setting");
        _loggingEnabled = enableLoggingService.GetLoggingState();
        _themeSetting =
            settingsService.GetSetting("Theme") as SettingIntVm
            ?? throw new Exception("Error retrieving Theme setting");
        _themeCollection = [.. themeService.Themes];
        _selectedTheme = _themeCollection.First(x => x.Id == _themeSetting.IntValue);

        // Update the theme setting selected theme value
        this.WhenAnyValue(x => x.SelectedTheme).Subscribe(x => ThemeSetting.IntValue = x.Id);

        var startMinimizedChanged = this.WhenAnyValue(
            x => x.StartMinimized.BoolValue,
            selector: val =>
                settingsService.GetSetting("StartMinimized") is not SettingBoolVm curVal
                || curVal.BoolValue != val
        );
        var loggingChanged = this.WhenAnyValue(
            x => x.LoggingEnabled,
            selector: val => val != enableLoggingService.GetLoggingState()
        );
        var startMenuChanged = this.WhenAnyValue(
            x => x.StartMenuChecked,
            selector: val => StartMenuEnabled && val != startMenuInstallerService.InstallStatus()
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
        ApplyCommand = ReactiveCommand.Create(
            () =>
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
                    && StartMenuChecked != startMenuInstallerService.InstallStatus()
                )
                {
                    if (StartMenuChecked)
                    {
                        startMenuInstallerService.Install();
                    }
                    else
                    {
                        startMenuInstallerService.Uninstall();
                    }
                }

                using var trn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                settingsService.UpdateSetting(StartMinimized);
                settingsService.UpdateSetting(ThemeSetting);
                trn.Complete();
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

    public SettingBoolVm StartMinimized
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
