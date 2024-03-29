using System;
using System.Runtime.Versioning;
using Splat;
using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Environment.Enums;
using YMouseButtonControl.Services.Environment.Interfaces;
using YMouseButtonControl.Services.MacOS.Implementations;
using YMouseButtonControl.Services.Windows.Implementations;

namespace YMouseButtonControl.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        RegisterCommonServices(services, resolver);
        RegisterPlatformSpecificServices(services, resolver);
    }

    private static void RegisterCommonServices(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.RegisterLazySingleton<IProfilesService>(
            () => new ProfilesService(resolver.GetRequiredService<IUnitOfWorkFactory>())
        );
    }

    private static void RegisterPlatformSpecificServices(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
        {
            RegisterWindowsServices(services, resolver);
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            RegisterMacOsServices(services, resolver);
        }
        else
        {
            throw new Exception("Unsupported operating system");
        }
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static void RegisterWindowsServices(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.RegisterLazySingleton<IProcessMonitorService>(() => new ProcessMonitorService());
        services.RegisterLazySingleton<ICurrentWindowService>(
            () => new Services.Windows.Implementations.CurrentWindowService()
        );
        services.RegisterLazySingleton<ILowLevelMouseHookService>(
            () =>
                new LowLevelMouseHookService(
                    resolver.GetRequiredService<IProfilesService>(),
                    resolver.GetRequiredService<ICurrentWindowService>()
                )
        );
        services.RegisterLazySingleton<IBackgroundTasksRunner>(
            () =>
                new Services.Windows.Implementations.BackgroundTasksRunner(
                    resolver.GetRequiredService<IMouseListener>(),
                    resolver.GetRequiredService<KeyboardSimulatorWorker>(),
                    resolver.GetRequiredService<ILowLevelMouseHookService>(),
                    resolver.GetRequiredService<ICurrentWindowService>()
                )
        );
        //services.RegisterLazySingleton<IPayloadInjectorService>(() => new PayloadInjectorService(
        //    resolver.GetRequiredService<IProfilesService>(),
        //    resolver.GetRequiredService<IProcessMonitorService>()
        //));
    }

    private static void RegisterMacOsServices(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.RegisterLazySingleton<IProcessMonitorService>(
            () => new MacOsProcessMonitorService()
        );
        services.RegisterLazySingleton<ICurrentWindowService>(
            () => new Services.MacOS.Implementations.CurrentWindowService()
        );
        services.RegisterLazySingleton<IBackgroundTasksRunner>(
            () =>
                new Services.MacOS.Implementations.BackgroundTasksRunner(
                    resolver.GetRequiredService<IMouseListener>(),
                    resolver.GetRequiredService<KeyboardSimulatorWorker>()
                )
        );
    }
}
