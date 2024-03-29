using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonPressedService : IAsMouseButtonPressedService
{
    private readonly ISimulateKeyService _simulateKeyService;

    public AsMouseButtonPressedService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void AsMouseButtonPressed(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        _simulateKeyService.TapKeys(mapping.Keys);
    }
}
