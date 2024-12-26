using System;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;
using YMouseButtonControl.Core.ViewModels.ProfilesList;

namespace YMouseButtonControl.Views.Dialogs;

public partial class ProcessSelectorDialog : ReactiveWindow<ProcessSelectorDialogViewModel>
{
    public ProcessSelectorDialog()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel != null)
                d(ViewModel!.OkCommand.Subscribe(Close));
        });
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
