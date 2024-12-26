using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.Layer;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class LayerView : ReactiveUserControl<LayerViewModel>
{
    public LayerView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            if (ViewModel is null)
            {
                return;
            }
            d(
                ViewModel.ShowSimulatedKeystrokesPickerInteraction.RegisterHandler(
                    ShowSimulateKeystrokesPicker
                )
            );
        });
    }

    private async Task ShowSimulateKeystrokesPicker(
        IInteractionContext<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokeVm?> context
    )
    {
        var dialog = new SimulatedKeystrokesDialog { DataContext = context.Input };

        var result = await dialog.ShowDialog<SimulatedKeystrokeVm?>(
            MainWindowProvider.GetMainWindow()
        );
        context.SetOutput(result);
    }
}
