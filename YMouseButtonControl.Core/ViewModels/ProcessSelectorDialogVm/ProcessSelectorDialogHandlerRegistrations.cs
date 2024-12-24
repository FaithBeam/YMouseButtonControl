using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm;

public static class ProcessSelectorDialogHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services.AddScoped<GetMaxProfileId.Handler>();

    public static void RegisterLinux(IServiceCollection services) =>
        services.AddScoped<IListProcessesHandler, ListProcessesLinux.Handler>();

    public static void RegisterOsx(IServiceCollection services) =>
        services.AddScoped<IListProcessesHandler, ListProcessesOsx.Handler>();

    [SupportedOSPlatform("windows5.1.2600")]
    public static void RegisterWindows(IServiceCollection services) =>
        services.AddScoped<IListProcessesHandler, ListProcessesWindows.Handler>();
}
