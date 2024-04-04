using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Windows.Implementations;

namespace YMouseButtonControl.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        RegisterCommonServices(services);
        RegisterPlatformSpecificServices(services);
    }

    private static void RegisterCommonServices(IServiceCollection services)
    {
        services.AddSingleton<IProfilesService, ProfilesService>();
    }

    private static void RegisterPlatformSpecificServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
        {
            RegisterWindowsServices(services);
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            RegisterMacOsServices(services);
        }
        else
        {
            throw new Exception("Unsupported operating system");
        }
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static void RegisterWindowsServices(IServiceCollection services)
    {
        services.AddSingleton<IProcessMonitorService, ProcessMonitorService>();
        services.AddTransient<
            ICurrentWindowService,
            Services.Windows.Implementations.CurrentWindowService
        >();
        services.AddTransient<
            IBackgroundTasksRunner,
            Services.Windows.Implementations.BackgroundTasksRunner
        >();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        services.AddSingleton<
            IProcessMonitorService,
            Services.MacOS.Implementations.MacOsProcessMonitorService
        >();
        services.AddSingleton<
            ICurrentWindowService,
            Services.MacOS.Implementations.CurrentWindowService
        >();
        services.AddSingleton<
            IBackgroundTasksRunner,
            Services.MacOS.Implementations.BackgroundTasksRunner
        >();
    }
}
