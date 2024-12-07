using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface IMouseComboViewModelFactory
{
    IMouseComboViewModel CreateWithMouseButton(MouseButton mouseButton, string labelTxt);
}

public class MouseComboViewModelFactory(
    IMouseListener mouseListener,
    IThemeService themeService,
    IProfilesService profilesService,
    IShowSimulatedKeystrokesDialogService showSimulatedKeystrokesDialogService
) : IMouseComboViewModelFactory
{
    public IMouseComboViewModel CreateWithMouseButton(MouseButton mouseButton, string labelTxt) =>
        new MouseComboViewModel(
            profilesService,
            mouseListener,
            themeService,
            mouseButton,
            showSimulatedKeystrokesDialogService
        )
        {
            LabelTxt = labelTxt,
        };
}
