using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonReleasedService : IAsMouseButtonReleasedService
{
    private readonly ISimulateKeyService _simulateKeyService;

    public AsMouseButtonReleasedService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void AsMouseButtonReleased(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Released)
        {
            return;
        }

        _simulateKeyService.TapKeys(mapping.Keys);
    }
}