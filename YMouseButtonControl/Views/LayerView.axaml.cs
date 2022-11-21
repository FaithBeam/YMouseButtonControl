using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Avalonia.Implementations;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Models;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class LayerView : ReactiveUserControl<LayerViewModel>
{
    public LayerView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            d(ViewModel.ShowSimulatedKeystrokesPickerInteraction.RegisterHandler(ShowSimulateKeystrokesPicker));
        });
    }

    private async Task ShowSimulateKeystrokesPicker(InteractionContext<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel?> context)
    {
        var dialog = new SimulatedKeystrokesDialog
        {
            DataContext = context.Input
        };

        var result = await dialog.ShowDialog<SimulatedKeystrokesDialogModel?>(MainWindowProvider.GetMainWindow());
        context.SetOutput(result);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}