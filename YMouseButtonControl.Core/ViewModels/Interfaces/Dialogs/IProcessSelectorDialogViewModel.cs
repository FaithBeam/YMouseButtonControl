using System.Windows.Input;

namespace YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

public interface IProcessSelectorDialogViewModel
{
    ICommand RefreshButtonCommand { get; }
}
