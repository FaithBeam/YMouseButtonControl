using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IGlobalSettingsDialogViewModel
{
    SettingBoolVm StartMinimized { get; set; }
}

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private SettingBoolVm _startMinimized;
    private readonly ObservableAsPropertyHelper<bool>? _applyIsExec;

    public GlobalSettingsDialogViewModel(ISettingsService settingsService)
    {
        _startMinimized =
            settingsService.GetBoolSetting("StartMinimized")
            ?? throw new Exception("Error retrieving StartMinimized setting");

        var startMinimizedChanged = this.WhenAnyValue(
            x => x.StartMinimized.Value,
            selector: val =>
            {
                if (settingsService.GetSetting("StartMinimized") is not SettingBoolVm curVal)
                {
                    return true;
                }

                return curVal.Value != val;
            }
        );

        var applyIsExecObs = this.WhenAnyValue(x => x.AppIsExec);
        var canSave = startMinimizedChanged.Merge(applyIsExecObs);
        ApplyCommand = ReactiveCommand.Create(
            () =>
            {
                settingsService.UpdateSetting(StartMinimized);

                settingsService.Save();
            },
            canSave
        );
        _applyIsExec = ApplyCommand.IsExecuting.ToProperty(this, x => x.AppIsExec);
    }

    public SettingBoolVm StartMinimized
    {
        get => _startMinimized;
        set => this.RaiseAndSetIfChanged(ref _startMinimized, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; init; }

    public bool AppIsExec => _applyIsExec?.Value ?? false;
}
