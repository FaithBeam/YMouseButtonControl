using Microsoft.Extensions.DependencyInjection;

namespace YMouseButtonControl.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IServiceCollection services)
    {
        ServicesBootstrapper.RegisterServices(services);
        DataAccessBootstrapper.RegisterDataAccess(services);
        KeyboardAndMouseBootstrapper.RegisterKeyboardAndMouse(services);
        FeaturesBootstrapper.RegisterFeatures(services);
        ViewModelsBootstrapper.RegisterViewModels(services);
        ViewBootstrapper.RegisterViews(services);
    }
}
