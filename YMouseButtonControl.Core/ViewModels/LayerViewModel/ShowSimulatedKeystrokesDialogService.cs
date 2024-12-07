using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface IShowSimulatedKeystrokesDialogService
{
    Interaction<
        SimulatedKeystrokesDialogViewModel,
        SimulatedKeystrokeVm?
    > ShowSimulatedKeystrokesPickerInteraction { get; }

    Task<SimulatedKeystrokeVm?> ShowSimulatedKeystrokesDialog(
        string buttonName,
        MouseButton mouseButton,
        BaseButtonMappingVm mapping
    );
}

public class ShowSimulatedKeystrokesDialogService(
    IMouseListener mouseListener,
    IThemeService themeService
) : IShowSimulatedKeystrokesDialogService
{
    public Interaction<
        SimulatedKeystrokesDialogViewModel,
        SimulatedKeystrokeVm?
    > ShowSimulatedKeystrokesPickerInteraction { get; } = new();

    public async Task<SimulatedKeystrokeVm?> ShowSimulatedKeystrokesDialog(
        string buttonName,
        MouseButton mouseButton,
        BaseButtonMappingVm? mapping
    )
    {
        return await ShowSimulatedKeystrokesPickerInteraction.Handle(
            new SimulatedKeystrokesDialogViewModel(
                mouseListener,
                themeService,
                buttonName,
                mouseButton,
                mapping as SimulatedKeystrokeVm
            )
        );
    }
}
