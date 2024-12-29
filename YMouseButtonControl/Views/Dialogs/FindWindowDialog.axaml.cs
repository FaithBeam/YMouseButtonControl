using Avalonia.ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

namespace YMouseButtonControl.Views.Dialogs;

public partial class FindWindowDialog : ReactiveWindow<FindWindowDialogVm>
{
    public FindWindowDialog()
    {
        InitializeComponent();
    }
}
