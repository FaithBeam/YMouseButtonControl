using System.Diagnostics;
using System.Reactive.Disposables;
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
            this.OneWayBind(ViewModel, vm => vm.Response!.Path, v => v.PathTxtBox.Text);

            CrosshairBtn
                .AddDisposableHandler(
                    PointerPressedEvent,
                    (_, _) =>
                    {
                        if (ViewModel != null)
                        {
                            ViewModel.CrosshairPressed = true;
                        }
                    },
                    RoutingStrategies.Tunnel
                )
                .DisposeWith(d);

            FindWindowDlgGrid
                .AddDisposableHandler(
                    PointerReleasedEvent,
                    (_, _) =>
                    {
                        if (ViewModel is not null)
                        {
                            ViewModel.CrosshairPressed = false;
                        }
                    },
                    RoutingStrategies.Tunnel
                )
                .DisposeWith(d);
        });
    }
}
