using System.Windows.Input;

namespace YMouseButtonControl.ViewModels.Interfaces.Dialogs;

public interface IProcessSelectorDialogViewModel
{
    ICommand RefreshButtonCommand { get; }
}
