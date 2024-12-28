using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;

namespace YMouseButtonControl.Core.ViewModels.Dialogs;

public static class FindWindowDialogHandlerRegistrations
{
    [SupportedOSPlatform("windows5.1.2600")]
    public static void RegisterWindows(IServiceCollection services) =>
        services.AddScoped<IWindowUnderCursorHandler, WindowUnderCursorWindows.Handler>();
}
