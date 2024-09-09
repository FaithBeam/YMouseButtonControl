using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.Views;

namespace YMouseButtonControl.DependencyInjection;

public static class ViewBootstrapper
{
    public static void RegisterViews(IServiceCollection services)
    {
        services.AddSingleton<IMainWindow, MainWindow>();
    }
}
