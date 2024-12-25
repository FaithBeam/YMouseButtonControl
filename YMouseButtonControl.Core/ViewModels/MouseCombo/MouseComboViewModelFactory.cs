using System.Collections.ObjectModel;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.MouseCombo.Queries.Theme;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.MouseCombo;

public interface IMouseComboViewModelFactory
{
    IMouseComboViewModel? CreateWithMouseButton(
        ProfileVm profileVm,
        MouseButton mouseButton,
        string labelTxt,
        ReadOnlyObservableCollection<BaseButtonMappingVm> buttonMappings,
        ReactiveUI.Interaction<
            SimulatedKeystrokesDialogViewModel,
            SimulatedKeystrokeVm?
        >? showSimulatedKeystrokesPickerInteraction
    );
}

public class MouseComboViewModelFactory(
    IMouseListener mouseListener,
    ISimulatedKeystrokesDialogVmFactory simulatedKeystrokesDialogVmFactory,
    GetThemeBackground.Handler getThemeBackgroundHandler,
    GetThemeHighlight.Handler getThemeHighlightHandler
) : IMouseComboViewModelFactory
{
    public IMouseComboViewModel CreateWithMouseButton(
        ProfileVm profileVm,
        MouseButton mouseButton,
        string labelTxt,
        ReadOnlyObservableCollection<BaseButtonMappingVm> buttonMappings,
        ReactiveUI.Interaction<
            SimulatedKeystrokesDialogViewModel,
            SimulatedKeystrokeVm?
        >? showSimulatedKeystrokesPickerInteraction
    ) =>
        new MouseComboViewModel(
            profileVm,
            mouseListener,
            simulatedKeystrokesDialogVmFactory,
            getThemeBackgroundHandler,
            getThemeHighlightHandler,
            mouseButton,
            buttonMappings,
            showSimulatedKeystrokesPickerInteraction!
        )
        {
            LabelTxt = labelTxt,
        };
}
