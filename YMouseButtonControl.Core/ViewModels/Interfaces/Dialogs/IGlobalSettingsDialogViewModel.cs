using YMouseButtonControl.Core.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

public interface IGlobalSettingsDialogViewModel
{
    Setting StartMinimized { get; set; }
}
