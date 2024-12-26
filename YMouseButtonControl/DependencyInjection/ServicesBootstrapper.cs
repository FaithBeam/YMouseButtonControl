using System;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.BackgroundTaskRunner;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Profiles.Queries.Profiles;
using YMouseButtonControl.Core.ViewModels.App;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.MouseCombo;
using YMouseButtonControl.Core.ViewModels.ProfilesList;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;
using YMouseButtonControl.Queries.Settings;

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
        AppHandlerRegistrations.RegisterCommon(services);
        ProcessSelectorDialogHandlerRegistrations.RegisterCommon(services);
        ProfilesListHandlerRegistrations.RegisterCommon(services);
        GlobalSettingsDialogHandlerRegistrations.RegisterCommon(services);
        MainWindowHandlerRegistrations.RegisterCommon(services);
        MouseComboHandlerRegistrations.RegisterCommon(services);
        MouseListenerHandlerRegistrations.RegisterCommon(services);
        SimulatedKeystrokesDialogHandlerRegistrations.RegisterCommon(services);
        services
            .AddScoped<ProfileVmConverter>()
            .AddScoped<ListDbProfiles.Handler>()
            .AddScoped<IProfilesCache, ProfilesCache>()
            .AddScoped<GetBoolSetting.Handler>()
            .AddScoped<IBackgroundTasksRunner, BackgroundTasksRunner>();
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
        AppHandlerRegistrations.RegisterLinux(services);
        GlobalSettingsDialogHandlerRegistrations.RegisterLinux(services);
        ProcessSelectorDialogHandlerRegistrations.RegisterLinux(services);
        if (Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "x11")
        {
            services.AddScoped<
                IGetCurrentWindow,
                Core.Services.KeyboardAndMouse.Implementations.MouseListener.Queries.CurrentWindow.GetCurrentWindowLinuxX11
            >();
        }
        else
        {
            services.AddScoped<IGetCurrentWindow, GetCurrentWindowLinux>();
        }
    }

    [SupportedOSPlatform("windows5.1.2600")]
    private static void RegisterWindowsServices(IServiceCollection services)
    {
        AppHandlerRegistrations.RegisterWindows(services);
        ProcessSelectorDialogHandlerRegistrations.RegisterWindows(services);
        GlobalSettingsDialogHandlerRegistrations.RegisterWindows(services);
        services.AddScoped<IGetCurrentWindow, GetCurrentWindowWindows>();
    }

    private static void RegisterMacOsServices(IServiceCollection services)
    {
        AppHandlerRegistrations.RegisterOsx(services);
        ProcessSelectorDialogHandlerRegistrations.RegisterOsx(services);
        GlobalSettingsDialogHandlerRegistrations.RegisterOsx(services);
        services.AddScoped<IGetCurrentWindow, GetCurrentWindowOsx>();
    }
}
