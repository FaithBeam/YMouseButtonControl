using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonPressedService : IAsMouseButtonPressedService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private readonly IParseKeysService _parseKeysService;

    public AsMouseButtonPressedService(ISimulateKeyService simulateKeyService, IParseKeysService parseKeysService)
    {
        _parseKeysService = parseKeysService;
        _simulateKeyService = simulateKeyService;
    }

    public void AsMouseButtonPressed(IButtonMapping mapping, bool pressed)
    {
        if (!pressed)
        {
            return;
        }

        _simulateKeyService.TapKeys(_parseKeysService.ParseKeys(mapping.Keys));
    }
}