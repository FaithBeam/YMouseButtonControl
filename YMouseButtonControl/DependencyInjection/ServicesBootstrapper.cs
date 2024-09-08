using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.Services;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Services.MacOS;
using YMouseButtonControl.Services.Windows;

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
        else if (OperatingSystem.IsMacOS())
        {
            RegisterMacOsServices(services);
        }
        else if (OperatingSystem.IsLinux())
        {
            RegisterLinuxServices(services);
        }
        else
        {
            throw new Exception("Unsupported operating system");
        }
    }

    private static void RegisterLinuxServices(IServiceCollection services)
    {
        services.AddSingleton<IProcessMonitorService, Services.Linux.ProcessMonitorService>();
        services.AddSingleton<ICurrentWindowService, Services.Linux.CurrentWindowService>();
        services.AddSingleton<IBackgroundTasksRunner, Services.Linux.BackgroundTasksRunner>();
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static void RegisterWindowsServices(IServiceCollection services)
    {
        services.AddSingleton<
            IStartupInstallerService,
            Services.Windows.Services.StartupInstallerService
        >();
        services.AddSingleton<IProcessMonitorService, ProcessMonitorService>();
        services.AddSingleton<ICurrentWindowService, Services.Windows.CurrentWindowService>();
        services.AddSingleton<IBackgroundTasksRunner, Services.Windows.BackgroundTasksRunner>();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        services.AddSingleton<IProcessMonitorService, MacOsProcessMonitorService>();
        services.AddSingleton<ICurrentWindowService, Services.MacOS.CurrentWindowService>();
        services.AddSingleton<IBackgroundTasksRunner, Services.MacOS.BackgroundTasksRunner>();
    }
}
