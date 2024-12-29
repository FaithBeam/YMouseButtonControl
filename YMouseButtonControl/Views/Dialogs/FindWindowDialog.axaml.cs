using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

namespace YMouseButtonControl;

public partial class FindWindowDialog : ReactiveWindow<FindWindowDialogVm>
{
    public FindWindowDialog()
    {
        InitializeComponent();
    }
}
