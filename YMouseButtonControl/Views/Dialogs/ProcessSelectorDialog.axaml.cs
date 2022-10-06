using System;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;

namespace YMouseButtonControl.Views.Dialogs;

public partial class ProcessSelectorDialog : ReactiveWindow<ProcessSelectorDialogViewModel>
{
    public ProcessSelectorDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            if (ViewModel != null) d(ViewModel!.OkCommand.Subscribe(Close));
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}