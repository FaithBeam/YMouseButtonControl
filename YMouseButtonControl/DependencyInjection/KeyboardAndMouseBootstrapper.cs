using SharpHook;
using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
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
        services.RegisterLazySingleton<ISimulateKeyService>(() => new SimulateKeyService(
            new EventSimulator()
        ));
        services.RegisterLazySingleton<IParseKeysService>(() => new ParseKeysService());
        services.RegisterLazySingleton<ISimulatedKeystrokesService>(() => new SimulatedKeystrokesService(
            resolver.GetRequiredService<ISimulateKeyService>(),
            resolver.GetRequiredService<IParseKeysService>()
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