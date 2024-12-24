using Microsoft.Extensions.DependencyInjection;

namespace YMouseButtonControl.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IServiceCollection services)
    {
        ServicesBootstrapper.RegisterServices(services);
        FactoriesBootstrapper.RegisterFactories(services);
        DataAccessBootstrapper.RegisterDataAccess(services);
        KeyboardAndMouseBootstrapper.RegisterKeyboardAndMouse(services);
        ViewModelsBootstrapper.RegisterViewModels(services);
        ViewBootstrapper.RegisterViews(services);
    }
}
