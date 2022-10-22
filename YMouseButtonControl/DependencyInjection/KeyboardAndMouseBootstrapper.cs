using SharpHook;
using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.SharpHook;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class KeyboardAndMouseBootstrapper
{
    public static void RegisterKeyboardAndMouse(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IMouseListener>(() => new MouseListener(
            new TaskPoolGlobalHook()
        ));
        services.RegisterLazySingleton<IKeyboardSimulator>(() => new KeyboardSimulator(new EventSimulator()));
        services.RegisterLazySingleton(() => new KeyboardSimulatorWorker(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<IMouseListener>(),
            resolver.GetRequiredService<IKeyboardSimulator>(),
            resolver.GetRequiredService<IProcessMonitorService>()
        ));
    }
}