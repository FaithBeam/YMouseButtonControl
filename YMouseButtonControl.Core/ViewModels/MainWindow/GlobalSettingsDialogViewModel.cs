using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IGlobalSettingsDialogViewModel;

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private SettingBoolVm _startMinimizedSetting;
    private SettingIntVm _themeSetting;
    private ObservableCollection<ThemeEnum> _themeCollection;
    private readonly ObservableAsPropertyHelper<bool>? _applyIsExec;

    public GlobalSettingsDialogViewModel(
        ISettingsService settingsService,
        IThemeService themeService
    )
        : base(themeService)
    {
        _startMinimizedSetting =
            settingsService.GetSetting("StartMinimized") as SettingBoolVm
            ?? throw new Exception("Error retrieving StartMinimized setting");
        _themeSetting =
            settingsService.GetSetting("Theme") as SettingIntVm
            ?? throw new Exception("Error retrieving Theme setting");
        _themeCollection = new ObservableCollection<ThemeEnum>
        {
            ThemeEnum.Default,
            ThemeEnum.Light,
            ThemeEnum.Dark,
        };

        var startMinimizedChanged = this.WhenAnyValue(
            x => x.StartMinimized.Value,
            selector: val =>
                settingsService.GetSetting("StartMinimized") is not SettingBoolVm curVal
                || curVal.Value != val
        );
        var themeChanged = this.WhenAnyValue(
            x => x.ThemeSetting.Value,
            selector: val =>
                settingsService.GetSetting("Theme") is not SettingIntVm curVal
                || curVal.Value != val
        );

        var applyIsExecObs = this.WhenAnyValue(x => x.AppIsExec);
        var canSave = startMinimizedChanged.Merge(applyIsExecObs).Merge(themeChanged);
        ApplyCommand = ReactiveCommand.Create(
            () =>
            {
                settingsService.UpdateSetting(StartMinimized);
                settingsService.UpdateSetting(ThemeSetting);

                settingsService.Save();
            },
            canSave
        );
        _applyIsExec = ApplyCommand.IsExecuting.ToProperty(this, x => x.AppIsExec);
    }

    private bool AppIsExec => _applyIsExec?.Value ?? false;

    public SettingBoolVm StartMinimized
    {
        get => _startMinimizedSetting;
        set => this.RaiseAndSetIfChanged(ref _startMinimizedSetting, value);
    }

    public SettingIntVm ThemeSetting
    {
        get => _themeSetting;
        set => this.RaiseAndSetIfChanged(ref _themeSetting, value);
    }

    public ObservableCollection<ThemeEnum> ThemeCollection
    {
        get => _themeCollection;
        set => this.RaiseAndSetIfChanged(ref _themeCollection, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; init; }
}
