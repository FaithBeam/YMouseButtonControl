using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonReleasedService(ISimulateKeyService simulateKeyService)
    : IAsMouseButtonReleasedService
{
    public void AsMouseButtonReleased(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Released)
        {
            return;
        }

        simulateKeyService.TapKeys(mapping.Keys);
    }
}
