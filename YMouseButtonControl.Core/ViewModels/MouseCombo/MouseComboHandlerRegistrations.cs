using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MouseCombo.Queries.Theme;

namespace YMouseButtonControl.Core.ViewModels.MouseCombo;

public static class MouseComboHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services.AddScoped<GetThemeBackground.Handler>().AddScoped<GetThemeHighlight.Handler>();
}
