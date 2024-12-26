using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.Logging;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.Settings;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.StartMenuInstall;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.StartMenuUninstall;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Queries.Logging;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Queries.Settings;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Queries.StartMenuInstallerStatus;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Queries.Themes;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog;

public static class GlobalSettingsDialogHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services)
    {
        services
            .AddScoped<GetThemeVariant.Handler>()
            .AddScoped<ListThemes.Handler>()
            .AddScoped<GetLoggingState.Handler>()
            .AddScoped<EnableLogging.Handler>()
            .AddScoped<DisableLogging.Handler>()
            .AddScoped<GetIntSetting.Handler>()
            .AddScoped<GetBoolSetting.Handler>()
            .AddScoped<UpdateSetting<string>.Handler>()
            .AddScoped<UpdateSetting<int>.Handler>()
            .AddScoped<UpdateSetting<bool>.Handler>();
    }

    public static void RegisterWindows(IServiceCollection services) =>
        services
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusWindows.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallWindows.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallWindows.Handler>();

    public static void RegisterLinux(IServiceCollection services) =>
        services
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusLinux.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallLinux.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallLinux.Handler>();

    public static void RegisterOsx(IServiceCollection services) =>
        services
            .AddScoped<IStartMenuInstallerStatusHandler, StartMenuInstallerStatusOsx.Handler>()
            .AddScoped<IStartMenuUninstallHandler, StartMenuUninstallOsx.Handler>()
            .AddScoped<IStartMenuInstallHandler, StartMenuInstallOsx.Handler>();
}
