using YMouseButtonControl.Core.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public interface IGlobalSettingsDialogViewModel
{
    Setting StartMinimized { get; set; }
}
