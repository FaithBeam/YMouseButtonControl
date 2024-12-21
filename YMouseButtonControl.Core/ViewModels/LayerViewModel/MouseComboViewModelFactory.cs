using System.Collections.ObjectModel;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface IMouseComboViewModelFactory
{
    IMouseComboViewModel? CreateWithMouseButton(
        ProfileVm profileVm,
        MouseButton mouseButton,
        string labelTxt,
        ReadOnlyObservableCollection<BaseButtonMappingVm> buttonMappings
    );
}

public class MouseComboViewModelFactory(
    IMouseListener mouseListener,
    IThemeService themeService,
    IShowSimulatedKeystrokesDialogService showSimulatedKeystrokesDialogService
) : IMouseComboViewModelFactory
{
    public IMouseComboViewModel CreateWithMouseButton(
        ProfileVm profileVm,
        MouseButton mouseButton,
        string labelTxt,
        ReadOnlyObservableCollection<BaseButtonMappingVm> buttonMappings
    ) =>
        new MouseComboViewModel(
            profileVm,
            mouseListener,
            themeService,
            mouseButton,
            showSimulatedKeystrokesDialogService,
            buttonMappings
        )
        {
            LabelTxt = labelTxt,
        };
}
