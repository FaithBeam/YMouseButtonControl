using Splat;
using YMouseButtonControl.DataAccess.UnitOfWork;
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
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonServices(services, resolver);
        RegisterPlatformSpecificServices(services, resolver);
    }

    private static void RegisterCommonServices(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IProfilesService>(() => new ProfilesService(
            resolver.GetRequiredService<IUnitOfWorkFactory>()
        ));
    }

    private static void RegisterPlatformSpecificServices(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        var platformService = resolver.GetRequiredService<IPlatformService>();
        var platform = platformService.GetPlatform();

        switch (platform)
        {
            case Platform.Windows:
                RegisterWindowsServices(services, resolver);
                break;
            case Platform.MacOs:
            case Platform.Linux:
                RegisterMacOSServices(services, resolver);
                break;
        }
    }

    private static void RegisterWindowsServices(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IProcessMonitorService>(() => new ProcessMonitorService());
        services.RegisterLazySingleton<ICurrentWindowService>(() => new CurrentWindowService());
        services.RegisterLazySingleton<ILowLevelMouseHookService>(() => new LowLevelMouseHookService(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ICurrentWindowService>()
        ));
        //services.RegisterLazySingleton<IPayloadInjectorService>(() => new PayloadInjectorService(
        //    resolver.GetRequiredService<IProfilesService>(),
        //    resolver.GetRequiredService<IProcessMonitorService>()
        //));
    }

    private static void RegisterMacOSServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IProcessMonitorService>(() => new MacOsProcessMonitorService());
    }
}