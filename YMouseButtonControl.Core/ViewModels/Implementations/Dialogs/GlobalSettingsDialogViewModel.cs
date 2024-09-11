using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private Setting _startMinimized;
    private readonly ObservableAsPropertyHelper<bool>? _applyIsExec;

    public GlobalSettingsDialogViewModel(ISettingsService settingsService)
    {
        _startMinimized =
            settingsService.GetSetting("StartMinimized")
            ?? throw new Exception($"Error retrieving StartMinimized setting");

        var startMinimizedChanged = this.WhenAnyValue(
            x => x.StartMinimized.Value,
            selector: val =>
            {
                var curVal = settingsService.GetSetting("StartMinimized");
                if (curVal is null)
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
                settingsService.UpdateSetting(StartMinimized.Id, StartMinimized.Value!);
            },
            canSave
        );
        _applyIsExec = ApplyCommand.IsExecuting.ToProperty(this, x => x.AppIsExec);
    }

    public Setting StartMinimized
    {
        get => _startMinimized;
        set => this.RaiseAndSetIfChanged(ref _startMinimized, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; init; }

    public bool AppIsExec => _applyIsExec?.Value ?? false;
}
