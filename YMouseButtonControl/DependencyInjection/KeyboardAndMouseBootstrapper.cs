using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.DependencyInjection;

public static class KeyboardAndMouseBootstrapper
{
    public static void RegisterKeyboardAndMouse(IServiceCollection services)
    {
        services.AddSingleton<IGlobalHook, SimpleGlobalHook>();
        services.AddSingleton<IMouseListener, MouseListener>();
        services.AddSingleton<IParseKeysService, ParseKeysService>();
        services.AddSingleton<IEventSimulator, EventSimulator>();
        services.AddSingleton<ISimulateKeyService, SimulateKeyService>();
        services.AddSingleton<IStickyHoldService, StickyHoldService>();
        services.AddSingleton<IAsMouseButtonPressedService, AsMouseButtonPressedService>();
        services.AddSingleton<IAsMouseButtonReleasedService, AsMouseButtonReleasedService>();
        services.AddSingleton<
            IDuringMousePressAndReleaseService,
            DuringMousePressAndReleaseService
        >();
        services.AddSingleton<IRepeatedWhileButtonDownService, RepeatedWhileButtonDownService>();
        services.AddSingleton<IStickyRepeatService, StickyRepeatService>();
        services.AddSingleton<ISimulatedKeystrokesService, SimulatedKeystrokesService>();
        services.AddSingleton<IRouteButtonMappingService, RouteButtonMappingService>();
        services.AddSingleton<IRouteMouseButtonService, RouteMouseButtonService>();
        services.AddSingleton<ISkipProfileService, SkipProfileService>();
        services.AddSingleton<KeyboardSimulatorWorker, KeyboardSimulatorWorker>();
    }
}
