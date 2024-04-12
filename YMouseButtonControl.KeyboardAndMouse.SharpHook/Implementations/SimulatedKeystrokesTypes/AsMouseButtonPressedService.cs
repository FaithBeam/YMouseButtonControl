using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonPressedService(ISimulateKeyService simulateKeyService)
    : IAsMouseButtonPressedService
{
    public void AsMouseButtonPressed(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        simulateKeyService.TapKeys(mapping.Keys);
    }
}
