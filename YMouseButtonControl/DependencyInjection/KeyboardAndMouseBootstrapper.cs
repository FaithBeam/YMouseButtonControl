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
        services.AddTransient<IMouseListener, MouseListener>();
        services.AddTransient<IEventSimulator, EventSimulator>();
        services.AddTransient<ISimulateKeyService, SimulateKeyService>();
        services.AddTransient<IStickyHoldService, StickyHoldService>();
        services.AddTransient<IAsMouseButtonPressedService, AsMouseButtonPressedService>();
        services.AddTransient<IAsMouseButtonReleasedService, AsMouseButtonReleasedService>();
        services.AddTransient<
            IDuringMousePressAndReleaseService,
            DuringMousePressAndReleaseService
        >();
        services.AddTransient<IRepeatedWhileButtonDownService, RepeatedWhileButtonDownService>();
        services.AddTransient<IStickyRepeatService, StickyRepeatService>();
        services.AddTransient<ISimulatedKeystrokesService, SimulatedKeystrokesService>();
        services.AddTransient<IRouteButtonMappingService, RouteButtonMappingService>();
        services.AddTransient<IRouteMouseButtonService, RouteMouseButtonService>();
        services.AddTransient<ISkipProfileService, SkipProfileService>();
        services.AddTransient<KeyboardSimulatorWorker>();
    }
}
