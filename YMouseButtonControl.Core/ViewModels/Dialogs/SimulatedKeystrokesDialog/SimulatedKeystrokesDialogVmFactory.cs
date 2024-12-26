using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog.Queries.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;

public interface ISimulatedKeystrokesDialogVmFactory
{
    SimulatedKeystrokesDialogViewModel Create(
        string buttonName,
        MouseButton mouseButton,
        BaseButtonMappingVm? mapping
    );
}

public class SimulatedKeystrokesDialogVmFactory(
    IMouseListener mouseListener,
    GetThemeVariant.Handler getThemeVariant
) : ISimulatedKeystrokesDialogVmFactory
{
    public SimulatedKeystrokesDialogViewModel Create(
        string buttonName,
        MouseButton mouseButton,
        BaseButtonMappingVm? mapping
    ) =>
        new(
            mouseListener,
            buttonName,
            mouseButton,
            getThemeVariant,
            mapping as SimulatedKeystrokeVm
        );
}
