using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MouseComboViewModel;

namespace YMouseButtonControl.DependencyInjection;

public static class FactoriesBootstrapper
{
    public static void RegisterFactories(IServiceCollection services)
    {
        services.AddScoped<IMouseComboViewModelFactory, MouseComboViewModelFactory>();
    }
}
