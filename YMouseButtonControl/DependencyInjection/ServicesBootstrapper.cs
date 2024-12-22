using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.Services.Logging;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.Services.StartupInstaller;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuInstall;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuUninstall;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.UpdateStartsMinimized;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Queries.StartsMinimized;

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
            .AddScoped<StartsMinimized.Handler>()
            .AddScoped<UpdateStartsMinimized.Handler>()
            .AddScoped<IEnableLoggingService, EnableLoggingService>()
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
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusLinux.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallLinux.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallLinux.Handler>()
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
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusWindows.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallWindows.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallWindows.Handler>()
            .AddScoped<IStartupInstallerService, Windows.Services.StartupInstallerService>()
            .AddScoped<IProcessMonitorService, Windows.Services.ProcessMonitorService>()
            .AddScoped<ICurrentWindowService, Windows.Services.CurrentWindowService>()
            .AddScoped<IBackgroundTasksRunner, Windows.Services.BackgroundTasksRunner>();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        services
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusOsx.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallOsx.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallOsx.Handler>()
            .AddScoped<IStartupInstallerService, MacOS.Services.StartupInstallerService>()
            .AddScoped<IProcessMonitorService, MacOS.Services.ProcessMonitorService>()
            .AddScoped<ICurrentWindowService, MacOS.Services.CurrentWindowService>()
            .AddScoped<IBackgroundTasksRunner, MacOS.Services.BackgroundTasksRunner>();
    }
}
