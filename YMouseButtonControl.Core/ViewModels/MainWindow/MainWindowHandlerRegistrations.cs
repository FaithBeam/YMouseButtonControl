using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Profiles;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public static class MainWindowHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services.AddScoped<IsCacheDirty.Handler>();
}
