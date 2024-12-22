using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Commands.StartupInstaller.Install;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Commands.StartupInstaller.Uninstall;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.CanBeInstalled;
using YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.IsInstalled;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel;

public static class AppHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) { }

    public static void RegisterLinux(IServiceCollection services)
    {
        services
            .AddScoped<IIsInstalledHandler, IsInstalledLinux.Handler>()
            .AddScoped<ICanBeInstalledHandler, CanBeInstalledLinux.Handler>()
            .AddScoped<IInstallHandler, InstallLinux.Handler>()
            .AddScoped<IUninstallHandler, UninstallLinux.Handler>();
    }

    public static void RegisterOsx(IServiceCollection services)
    {
        services
            .AddScoped<IIsInstalledHandler, IsInstalledOsx.Handler>()
            .AddScoped<ICanBeInstalledHandler, CanBeInstalledOsx.Handler>()
            .AddScoped<IInstallHandler, InstallOsx.Handler>()
            .AddScoped<IUninstallHandler, UninstallOsx.Handler>();
    }

    [SupportedOSPlatform("windows5.1.2600")]
    public static void RegisterWindows(IServiceCollection services)
    {
        services
            .AddScoped<IIsInstalledHandler, IsInstalledWindows.Handler>()
            .AddScoped<ICanBeInstalledHandler, CanBeInstalledWindows.Handler>()
            .AddScoped<IInstallHandler, InstallWindows.Handler>()
            .AddScoped<IUninstallHandler, UninstallWindows.Handler>();
    }
}
