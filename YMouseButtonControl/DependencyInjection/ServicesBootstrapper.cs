﻿using ORMi;
using Splat;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Environment.Enums;
using YMouseButtonControl.Services.Environment.Interfaces;
using YMouseButtonControl.Services.Windows.Implementations;
using YMouseButtonControl.ViewModels.Services.Implementations;
using YMouseButtonControl.ViewModels.Services.Interfaces;

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
        services.RegisterLazySingleton<ICurrentProfileOperationsMediator>(() => new CurrentCurrentProfileOperationsMediator());
        services.Register<ICheckDefaultProfileService>(() => new CheckDefaultProfileService(
            resolver.GetRequiredService<IUnitOfWorkFactory>(),
            resolver.GetRequiredService<ICurrentProfileOperationsMediator>()
        ));
    }
    
    private static void RegisterPlatformSpecificServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        var platformService = resolver.GetRequiredService<IPlatformService>();
        var platform = platformService.GetPlatform();

        if (platform is Platform.Windows)
        {
            RegisterWindowsServices(services, resolver);
        }
    }

    private static void RegisterWindowsServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IProcessMonitorService>(() => new ProcessMonitorService());
    }
}