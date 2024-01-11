using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;

namespace YMouseButtonControl.Views.Dialogs;

public partial class SimulatedKeystrokesDialog : ReactiveWindow<SimulatedKeystrokesDialogViewModel>
{
    public SimulatedKeystrokesDialog()
    {
        InitializeComponent();

        this.WhenActivated(delegate(Action<IDisposable> action)
        {
            action(ViewModel!.OkCommand.Subscribe(Close));
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}