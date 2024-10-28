using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class MouseButtonComboControl : ReactiveUserControl<IMouseComboViewModel>
{
    private readonly IMainWindowProvider _mainWindowProvider;

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
        _mainWindowProvider = Program.Container!.GetRequiredService<IMainWindowProvider>();
    }

    private async Task ShowSimulateKeystrokesPicker(
        IInteractionContext<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokeVm?> context
    )
    {
        var dialog = new SimulatedKeystrokesDialog { DataContext = context.Input };

        var result = await dialog.ShowDialog<SimulatedKeystrokeVm?>(
            _mainWindowProvider.GetMainWindow()
        );
        context.SetOutput(result);
    }
}
