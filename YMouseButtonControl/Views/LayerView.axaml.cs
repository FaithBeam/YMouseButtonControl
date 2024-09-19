using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
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
                ViewModel.ShowSimulatedKeystrokesDialogService.ShowSimulatedKeystrokesPickerInteraction.RegisterHandler(
                    ShowSimulateKeystrokesPicker
                )
            );
        });
    }

    private static async Task ShowSimulateKeystrokesPicker(
        IInteractionContext<SimulatedKeystrokesDialogViewModel?, SimulatedKeystrokeVm?> context
    )
    {
        var dialog = new SimulatedKeystrokesDialog { DataContext = context.Input };

        var result = await dialog.ShowDialog<SimulatedKeystrokeVm?>(
            MainWindowProvider.GetMainWindow()
        );
        context.SetOutput(result);
    }
}
