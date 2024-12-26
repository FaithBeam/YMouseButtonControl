using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog.Queries.Theme;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;

public static class SimulatedKeystrokesDialogHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services.AddScoped<GetThemeVariant.Handler>();
}
