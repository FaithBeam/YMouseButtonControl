using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
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

public class SimulatedKeystrokesDialogVmFactory(IMouseListener mouseListener)
    : ISimulatedKeystrokesDialogVmFactory
{
    public SimulatedKeystrokesDialogViewModel Create(
        string buttonName,
        MouseButton mouseButton,
        BaseButtonMappingVm? mapping
    ) => new(mouseListener, buttonName, mouseButton, mapping as SimulatedKeystrokeVm);
}
