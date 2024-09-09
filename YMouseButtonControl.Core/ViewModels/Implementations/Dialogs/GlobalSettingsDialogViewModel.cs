using System;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Profiles.Implementations;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public class GlobalSettingsDialogViewModel : DialogBase, IGlobalSettingsDialogViewModel
{
    private readonly ISettingsService _settingsService;
    private Setting _startMinimized;

    public GlobalSettingsDialogViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        _startMinimized =
            _settingsService.GetSetting("StartMinimized")
            ?? throw new Exception($"Error retrieving StartMinimized setting");
    }

    public Setting StartMinimized
    {
        get => _startMinimized;
        set => this.RaiseAndSetIfChanged(ref _startMinimized, value);
    }
}
