using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Services.MacOS;
using YMouseButtonControl.Services.Windows;
using BackgroundTasksRunner = YMouseButtonControl.Services.MacOS.BackgroundTasksRunner;
using CurrentWindowService = YMouseButtonControl.Services.MacOS.CurrentWindowService;

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
        services.AddTransient<ICurrentWindowService, Services.Windows.CurrentWindowService>();
        services.AddTransient<IBackgroundTasksRunner, Services.Windows.BackgroundTasksRunner>();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        services.AddSingleton<IProcessMonitorService, MacOsProcessMonitorService>();
        services.AddSingleton<ICurrentWindowService, CurrentWindowService>();
        services.AddSingleton<IBackgroundTasksRunner, BackgroundTasksRunner>();
    }
}
