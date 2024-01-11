using SharpHook;
using SharpHook.Reactive;
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
            new SimpleReactiveGlobalHook()
        ));
        services.RegisterLazySingleton<ISimulateKeyService>(() => new SimulateKeyService(
            new EventSimulator()
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
        services.RegisterLazySingleton<IRepeatedWhileButtonDownService>(() => new RepeatedWhileButtonDownService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<IStickyRepeatService>(() => new StickyRepeatService(
            resolver.GetRequiredService<ISimulateKeyService>()
        ));
        services.RegisterLazySingleton<ISimulatedKeystrokesService>(() => new SimulatedKeystrokesService(
            resolver.GetRequiredService<ISimulateKeyService>(),
            resolver.GetRequiredService<IStickyHoldService>(),
            resolver.GetRequiredService<IAsMouseButtonPressedService>(),
            resolver.GetRequiredService<IAsMouseButtonReleasedService>(),
            resolver.GetRequiredService<IDuringMousePressAndReleaseService>(),
            resolver.GetRequiredService<IRepeatedWhileButtonDownService>(),
            resolver.GetRequiredService<IStickyRepeatService>()
        ));
        services.RegisterLazySingleton<IMouseCoordinatesService>(() => new MouseCoordinatesService(
            resolver.GetRequiredService<IMouseListener>()
            ));
        services.RegisterLazySingleton<IRouteButtonMappingService>(() => new RouteButtonMappingService(
            resolver.GetRequiredService<ISimulatedKeystrokesService>(),
            resolver.GetRequiredService<IMouseCoordinatesService>()
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