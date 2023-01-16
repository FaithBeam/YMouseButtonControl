using SharpHook;
using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class KeyboardAndMouseBootstrapper
{
    public static void RegisterKeyboardAndMouse(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IMouseListener>(() => new MouseListener(
            new TaskPoolGlobalHook()
        ));
        services.RegisterLazySingleton<IParseKeysService>(() => new ParseKeysService());
        services.RegisterLazySingleton<ISimulateKeyService>(() => new SimulateKeyService(
            new EventSimulator(),
            resolver.GetRequiredService<IParseKeysService>()
        ));
        services.RegisterLazySingleton<IStickyHoldService>(() => new StickyHoldService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<IAsMouseButtonPressedService>(() => new AsMouseButtonPressedService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<IAsMouseButtonReleasedService>(() => new AsMouseButtonReleasedService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<IDuringMousePressAndReleaseService>(() => new DuringMousePressAndReleaseService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<ISimulatedKeystrokesService>(() => new SimulatedKeystrokesService(
            resolver.GetRequiredService<ISimulateKeyService>(),
            resolver.GetRequiredService<IParseKeysService>(),
            resolver.GetRequiredService<IStickyHoldService>(),
            resolver.GetRequiredService<IAsMouseButtonPressedService>(),
            resolver.GetRequiredService<IAsMouseButtonReleasedService>(),
            resolver.GetRequiredService<IDuringMousePressAndReleaseService>()
        ));
        services.RegisterLazySingleton<IRouteButtonMappingService>(() => new RouteButtonMappingService(
            resolver.GetRequiredService<ISimulatedKeystrokesService>()
        ));
        services.RegisterLazySingleton<IRouteMouseButtonService>(() => new RouteMouseButtonService(
            resolver.GetRequiredService<IRouteButtonMappingService>()
        ));
        services.RegisterLazySingleton<ISkipProfileService>(() => new SkipProfileService(
            resolver.GetRequiredService<ICurrentWindowService>()
        ));
        services.RegisterLazySingleton(() => new KeyboardSimulatorWorker(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<IMouseListener>(),
            resolver.GetRequiredService<IRouteMouseButtonService>(),
            resolver.GetRequiredService<ISkipProfileService>()
        ));
    }
}