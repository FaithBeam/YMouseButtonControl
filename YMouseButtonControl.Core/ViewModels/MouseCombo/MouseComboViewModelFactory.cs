using System.Collections.ObjectModel;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.Models;
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
    IThemeService themeService,
    ISimulatedKeystrokesDialogVmFactory simulatedKeystrokesDialogVmFactory
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
            themeService,
            mouseButton,
            buttonMappings,
            showSimulatedKeystrokesPickerInteraction!
        )
        {
            LabelTxt = labelTxt,
        };
}
