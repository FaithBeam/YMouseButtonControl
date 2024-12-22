using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.MouseComboViewModel;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class MouseButtonComboControl : ReactiveUserControl<IMouseComboViewModel>
{
    public MouseButtonComboControl()
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
