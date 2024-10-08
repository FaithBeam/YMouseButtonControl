using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.StartupInstaller;
using YMouseButtonControl.Core.Services.Theme;

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
        services
            .AddScoped<IThemeService, ThemeService>()
            .AddScoped<IProfilesService, ProfilesService>()
            .AddScoped<ISettingsService, SettingsService>();
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
        services
            .AddScoped<IStartupInstallerService, Linux.Services.StartupInstallerService>()
            .AddScoped<IProcessMonitorService, Linux.Services.ProcessMonitorService>()
            .AddScoped<IBackgroundTasksRunner, Linux.Services.BackgroundTasksRunner>();
        if (Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "x11")
        {
            services.AddScoped<ICurrentWindowService, Linux.Services.CurrentWindowServiceX11>();
        }
        else
        {
            services.AddScoped<ICurrentWindowService, Linux.Services.CurrentWindowService>();
        }
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static void RegisterWindowsServices(IServiceCollection services)
    {
        services
            .AddScoped<IStartupInstallerService, Windows.Services.StartupInstallerService>()
            .AddScoped<IProcessMonitorService, Windows.Services.ProcessMonitorService>()
            .AddScoped<ICurrentWindowService, Windows.Services.CurrentWindowService>()
            .AddScoped<IBackgroundTasksRunner, Windows.Services.BackgroundTasksRunner>();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        services
            .AddScoped<IStartupInstallerService, MacOS.Services.StartupInstallerService>()
            .AddScoped<IProcessMonitorService, MacOS.Services.ProcessMonitorService>()
            .AddScoped<ICurrentWindowService, MacOS.Services.CurrentWindowService>()
            .AddScoped<IBackgroundTasksRunner, MacOS.Services.BackgroundTasksRunner>();
    }
}