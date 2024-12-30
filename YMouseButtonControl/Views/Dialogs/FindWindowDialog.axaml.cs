using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;

namespace YMouseButtonControl.Views.Dialogs;

public partial class FindWindowDialog : ReactiveWindow<FindWindowDialogVm>
{
    public FindWindowDialog()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            CrosshairBtn
                .AddDisposableHandler(
                    PointerPressedEvent,
                    CrosshairBtn_OnPointerPressed,
                    RoutingStrategies.Tunnel
                )
                .DisposeWith(d);

            FindWindowDlgGrid
                .AddDisposableHandler(
                    PointerReleasedEvent,
                    FindWindowDlgGrid_OnPointerReleased,
                    RoutingStrategies.Tunnel
                )
                .DisposeWith(d);
        });
    }

    private void CrosshairBtn_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (ViewModel is not null)
        {
            ViewModel.CrosshairPressed = true;
        }
    }

    private void FindWindowDlgGrid_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (ViewModel is not null)
        {
            ViewModel.CrosshairPressed = false;
        }
    }
}
